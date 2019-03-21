using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Testing;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base.Web.Abstractions {

    public class SecureApiClient : ApiClient {

        SecureTokenCache _secureTokenCache;
        ApiClient _identityServerApiClient;
        IHostingEnvironment _hostingEnvironment;

        public SecureApiClient(HttpClient httpClient,
            ApiClient identityServerApiClient,
            IConfiguration config,
            ScopeProperties scopeProperties,
            SecureTokenCache secureTokenCache,
            IHostingEnvironment hostingEnvironment)
            : base(httpClient, config, scopeProperties) {

            _secureTokenCache = secureTokenCache;
            _identityServerApiClient = identityServerApiClient;
            _hostingEnvironment = hostingEnvironment;

            SetBearerToken(config).Wait();
        }


        private async Task SetBearerToken(IConfiguration config) {
            List<ApiConfig> apis = new List<ApiConfig>();

            config.GetSection("Apis").Bind(apis);

            var targetApi = apis
                .Where(x => x.BaseAddress == HttpClient.BaseAddress.ToString())
                .FirstOrDefault();

            TokenResponse tokenResponse = null;

            //try to get the access token from the cache
            _secureTokenCache.TryGetValue(targetApi.ProjectName, out CachedToken cachedToken);
            if (cachedToken != null && cachedToken.Expiration < DateTime.Now.Subtract(TimeSpan.FromSeconds(10))) {
                tokenResponse = cachedToken.TokenResponse;
            }
            else {
                var identityServerApi = apis
                    .Where(x => x.BaseAddress == _identityServerApiClient.HttpClient.BaseAddress.ToString())
                    .FirstOrDefault();

                //get a new token
                tokenResponse = await GetTokenResponse(targetApi, identityServerApi);

                //update cache
                cachedToken = new CachedToken {
                    TokenResponse = tokenResponse,
                    Expiration = DateTime.Now.Add(TimeSpan.FromSeconds(tokenResponse.ExpiresIn))
                };
                _secureTokenCache[targetApi.ProjectName] = cachedToken;

            }

            HttpClient.SetBearerToken(tokenResponse.AccessToken);

            if (ScopeProperties.User != null)
                HttpClient.DefaultRequestHeaders.Add("User", ScopeProperties.User);

        }



        private async Task<TokenResponse> GetTokenResponse(ApiConfig targetApi, ApiConfig identityServerApi) {


            var disco = await _identityServerApiClient.HttpClient.GetDiscoveryDocumentAsync(identityServerApi.BaseAddress);
            if (disco.IsError) {
                throw new ApplicationException($"Cannot retrieve discovery document from {identityServerApi.BaseAddress}");
            }

            // request token
            var tokenResponse = await _identityServerApiClient.HttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
                Address = disco.TokenEndpoint,

                ClientId = _hostingEnvironment.ApplicationName,
                ClientSecret = targetApi.IdentityServerSecret,
                Scope = string.Join(' ', targetApi.Scopes)
            });

            if (tokenResponse.IsError) {
                throw new ApplicationException($"Cannot retrieve token from {identityServerApi.BaseAddress}");
            }

            return tokenResponse;

        }


    }
}

