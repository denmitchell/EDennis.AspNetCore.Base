using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Testing;
using Flurl;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base.Web.Abstractions {

    public class SecureApiClient : ApiClient {

        private readonly SecureTokenCache _secureTokenCache;
        private readonly ApiClient _identityServerApiClient;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SecureApiClient(HttpClient httpClient,
            IConfiguration config,
            ScopeProperties scopeProperties,
            ApiClient identityServerApiClient,
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
            var clientSecret = config["Security:ClientSecret"];

            if (string.IsNullOrEmpty(clientSecret))
                throw new ApplicationException($"Configuration for {_hostingEnvironment.ApplicationName} is missing its Secret (string) setting");

            var targetApi = apis
                .Where(x => CleanUrl(x.BaseAddress) == CleanUrl(HttpClient.BaseAddress))
                .FirstOrDefault();

            if (targetApi.Scopes == null || targetApi.Scopes.Count() == 0)
                throw new ApplicationException($"Configuration for {targetApi.ProjectName} is missing its Scopes (string[]) setting");


            TokenResponse tokenResponse = null;

            //try to get the access token from the cache
            _secureTokenCache.TryGetValue(targetApi.ProjectName, out CachedToken cachedToken);
            if (cachedToken != null && cachedToken.Expiration < DateTime.Now.Subtract(TimeSpan.FromSeconds(10))) {
                tokenResponse = cachedToken.TokenResponse;
            }
            else {
                var identityServerApi = apis
                    .Where(x => CleanUrl(x.BaseAddress) == CleanUrl(_identityServerApiClient.HttpClient.BaseAddress))
                    .FirstOrDefault();

                //get a new token
                tokenResponse = await GetTokenResponse(targetApi, identityServerApi, clientSecret);

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

        private string CleanUrl(string url) {
            return Regex.Replace(Regex.Replace(url, "/$", ""), "\\$", "");
        }
        private string CleanUrl(Uri uri) {
            return Regex.Replace(Regex.Replace(uri.ToString(), "/$", ""), "\\$", "");
        }



        private async Task<TokenResponse> GetTokenResponse(ApiConfig targetApi, ApiConfig identityServerApi, string clientSecret) {


            var disco = await _identityServerApiClient.HttpClient.GetDiscoveryDocumentAsync(identityServerApi.BaseAddress);
            if (disco.IsError) {
                throw new ApplicationException($"Cannot retrieve discovery document from {identityServerApi.BaseAddress}");
            }

            // request token
            var tokenResponse = await _identityServerApiClient.HttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
                Address = disco.TokenEndpoint,

                ClientId = _hostingEnvironment.ApplicationName,
                ClientSecret = clientSecret,
                Scope = string.Join(' ', targetApi.Scopes)
            });

            if (tokenResponse.IsError) {
                throw new ApplicationException($"Cannot retrieve token from {identityServerApi.BaseAddress}");
            }

            return tokenResponse;

        }


    }
}

