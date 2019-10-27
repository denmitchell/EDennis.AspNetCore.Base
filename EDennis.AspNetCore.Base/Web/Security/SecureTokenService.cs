using EDennis.AspNetCore.Base.Web.Abstractions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web.Security {

    /// <summary>
    /// This service should be registered as a singleton because it maintains a cache.
    /// This service does several things:
    ///   (1) Retrieves tokens from an IdentityServer4
    ///   (2) Stores tokens in a cache to prevent needless calls to IdentityServer4.
    ///       NOTE: the expiration is 1 minute before the expiration configured in IdentityServer4
    ///   (3) Pings IdentityServer4 (refreshes the DiscoveryDocumentResponse) on a pre-configured
    ///       frequency in order to prevent idle timeouts from delaying token generation
    /// </summary>
    public class SecureTokenService : IDisposable {

        private readonly HttpClient _httpClient;
        private readonly int _pingFrequency;
        private DiscoveryDocumentResponse _disco;
        private readonly string _clientSecret;
        private Timer _timer;
        private readonly IWebHostEnvironment _environment;
        private readonly Profiles _profiles;

        /// <summary>
        /// This buffer period is used to explicitly invalidate a token that is requested
        /// after a token expires in the current cache but before the token is due to 
        /// expire in IdentityServer.  The buffer minimizes the likelihood that a cached
        /// token that is supposedly still valid is actually expired in IdentityServer. 
        /// This could happen if the system clocks for the two servers are out of sync.
        /// </summary>
        public static readonly TimeSpan TOKEN_EXPIRATION_BUFFER_PERIOD = new TimeSpan(0, 0, 15);

        protected ILogger Logger { get; }

        //outer key is profile, inner key is the ApiClientType
        private readonly Dictionary<string, Dictionary<string, CachedToken>> _tokenCache 
            = new Dictionary<string,Dictionary<string, CachedToken>>();

        #region public api
        public SecureTokenService(HttpClient httpClient, ILogger<SecureTokenService> logger, 
            IOptionsMonitor<SecurityOptions> options,
            IOptionsMonitor<Profiles> profiles, IWebHostEnvironment environment) {

            Logger = logger;
            
            _profiles = profiles.CurrentValue;
            _httpClient = httpClient;
            var currentOptions = options.CurrentValue;
            _httpClient.BaseAddress = new Uri(currentOptions.IdentityServerApi);
            _clientSecret = currentOptions.ClientSecret;
            _pingFrequency = currentOptions.IdentityServerPingFrequency;
            _environment = environment;

            if(_pingFrequency > 0)
                SchedulePing();
        }



        /// <summary>
        /// Obtains an OAuth access token from either the cache or IdentityServer
        /// </summary>
        /// <typeparam name="TClient">the type of SecureApiClient for whom to obtain the token</typeparam>
        /// <param name="client">the SecureApiClient for whom to obtain the token</param>
        /// <param name="profileName">The scope profile that the SecureApiClient uses</param>
        /// <param name="scopeProfileInstruction">the SPI instruction to transmit securely to 
        /// the called API via requested scope</param>
        /// <returns>access token as an IdentityModel TokenResponse object</returns>
        public async Task<TokenResponse> GetToken<TClient>(TClient client, string profileName = "Default",
                string scopeProfileInstruction = null)
            where TClient : SecureApiClient {
            TokenResponse tokenResponse;
            CachedToken cachedToken;

            //build the logger scope for comprehensive logging capabilities
            IEnumerable<KeyValuePair<string, object>> loggerScope = new List<KeyValuePair<string, object>> {
                KeyValuePair.Create<string,object>("IdentityServerUrl",_httpClient.BaseAddress.ToString()),
                KeyValuePair.Create<string,object>("ApiClientName",client.Name),
                KeyValuePair.Create<string,object>("ApiClientUrl",client.HttpClient.BaseAddress.ToString()),
                KeyValuePair.Create<string,object>("ProfileName",profileName),
                KeyValuePair.Create<string,object>("ScopeProfileInstruction",scopeProfileInstruction)
            };


            using (Logger.BeginScope(loggerScope)) {

                //if the cache has an unexpired token for the profile and client, return it.
                if (_tokenCache.ContainsKey(profileName)) {
                    var cachedTokens = _tokenCache[profileName];
                    if (cachedTokens.ContainsKey(client.Name)) {
                        cachedToken = cachedTokens[client.Name];
                        if (cachedToken != null && cachedToken.Expiration < DateTime.Now) {
                            //note: don't update cache expiration, because IdentityServer's expiration isn't updated
                            Logger.LogDebug("Obtaining cached security token");
                            tokenResponse = cachedToken.TokenResponse;
                            return tokenResponse;
                        }
                        else if (cachedToken != null) {
                            //if expired within buffer period, explicitly invalidate the old token, just
                            //   in case the old token is still valid and would be retrieved again 
                            if(cachedToken.Expiration.Add(TOKEN_EXPIRATION_BUFFER_PERIOD) < DateTime.Now)
                                await InvalidateToken(cachedToken.TokenResponse);
                        }
                    }
                }


                //the cache doesn't have an appropriate token, so ...

                //get a new token
                tokenResponse = await GetTokenResponse(client.Name, profileName, scopeProfileInstruction);

                //update cache
                Logger.LogDebug("Updating security token cache");
                cachedToken = new CachedToken {
                    TokenResponse = tokenResponse,
                    //expire earlier than IdentityServer, just in case server clocks get out of sync.
                    Expiration = DateTime.Now
                        .Add(TimeSpan.FromSeconds(tokenResponse.ExpiresIn))
                        .Subtract(TOKEN_EXPIRATION_BUFFER_PERIOD)
                };
                if (!_tokenCache.ContainsKey(profileName))
                    _tokenCache.Add(profileName, new Dictionary<string, CachedToken>());
                if (!_tokenCache[profileName].ContainsKey(client.Name))
                    _tokenCache[profileName].Add(client.Name, cachedToken);
                else
                    _tokenCache[profileName][client.Name] = cachedToken;

            }
            return tokenResponse;

        }

        private async Task InvalidateToken(TokenResponse tokenResponse) {

            await _httpClient.RevokeTokenAsync(new TokenRevocationRequest {
                Address = "https://demo.identityserver.io/connect/revocation",
                ClientId = "client",
                ClientSecret = "secret",

                Token = tokenResponse.AccessToken
            });

        }


        #endregion
        #region private methods and classes

        /// <summary>
        /// Gets the OAuth Discovery Document from IdentityServer4.  This method can be called
        /// at a regular frequency to prevent the server from slipping into Idle mode.
        /// </summary>
        /// <returns>Task</returns>
        private async Task GetDiscoveryDocumentResponse() {
            IEnumerable<KeyValuePair<string, object>> loggerScope = new List<KeyValuePair<string, object>> {
                KeyValuePair.Create<string,object>("IdentityServerUrl",_httpClient.BaseAddress.ToString()),
            };
            using (Logger.BeginScope(loggerScope)) {
                Logger.LogDebug("Retrieving discovery document from Identity Server.");
                _disco = await _httpClient.GetDiscoveryDocumentAsync(_httpClient.BaseAddress.ToString());
                if (_disco.IsError) {
                    var ex = new ApplicationException($"Cannot retrieve discovery document from Identity Server.");
                    Logger.LogError(ex, ex.Message);
                    throw ex;
                }
            }
            return;
        }



        /// <summary>
        /// Gets a new token from IdentityServer
        /// </summary>
        /// <param name="apiClientName">The name of the SecureApiClient for which to obtain the token</param>
        /// <param name="profileName">The (active) profile that contains the url for the SecureApiClient</param>
        /// <param name="scopeProfileInstruction">An instruction for the API to use a particular profile when accessed by the client</param>
        /// <returns></returns>
        private async Task<TokenResponse> GetTokenResponse(string apiClientName, string profileName,
            string scopeProfileInstruction) {

            //privileged scope because this is internal API.
            var apiScope = $"{_profiles[profileName ?? default].Apis[apiClientName]}.*";

            //add ScopeProfileInstruction to scope, when present
            string scope = apiScope;
            if (scopeProfileInstruction != null)
                scope += $" spi:{scopeProfileInstruction}";


            Logger.LogDebug("Obtaining new security token...");

            // request token
            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
                Address = _disco.TokenEndpoint,

                ClientId = _environment.ApplicationName,
                ClientSecret = _clientSecret,
                Scope = scope,
                 
            });


            if (tokenResponse.IsError) {
                var ex = new ApplicationException($"Cannot retrieve token from Identity Server");
                Logger.LogError(ex, ex.Message);
                throw ex;
            } else {
                Logger.LogDebug("Retrieved new security token with expiration = {TokenExpiration} seconds", tokenResponse.ExpiresIn);
            }

            return tokenResponse;

        }


        /// <summary>
        /// Schedules Identity Server to be pinged -- retrieving a new Discovery Document --
        /// at the _pingFrequency interval
        /// </summary>
        private void SchedulePing() {
            var autoEvent = new AutoResetEvent(false);
            var callbackInvoker = new CallbackInvoker(GetDiscoveryDocumentResponse);

            _timer = new Timer(callbackInvoker.InvokeCallback,
                               autoEvent, 0, _pingFrequency * 1000 * 60);
        }



        /// <summary>
        /// Invokes a callback method, typically from a Timer object.
        /// </summary>
        class CallbackInvoker {
            private readonly Func<Task> callback;

            public CallbackInvoker(Func<Task> callback) {
                this.callback = callback;
            }

            // This method is called by the timer delegate.
            #pragma warning disable IDE0060 // Remove unused parameter
            public void InvokeCallback(Object stateInfo) {
                callback();
            }
        }

        #endregion
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects).
                    _timer.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion


    }

}
