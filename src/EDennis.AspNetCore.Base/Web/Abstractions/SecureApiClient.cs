using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Web {

    public abstract class SecureApiClient : ApiClient, ISecureApiClient {

        public SecureApiClient(IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis, IScopeProperties scopeProperties,
            ISecureTokenService secureTokenService, 
            IWebHostEnvironment env, ILogger logger)
            : base(httpClientFactory, apis, scopeProperties, env, logger) {

            var tokenResponse = secureTokenService.GetTokenAsync(this).Result;
            HttpClient.SetBearerToken(tokenResponse.AccessToken);
        }


    }
}
