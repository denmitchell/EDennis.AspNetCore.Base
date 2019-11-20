using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Web {

    public abstract class SecureApiClient : ApiClient, ISecureApiClient {

        public SecureApiClient(HttpClient httpClient,
            IOptionsMonitor<Apis> apis, IScopeProperties scopeProperties,
            ISecureTokenService secureTokenService, ILogger logger, IScopedLogger scopedLogger)
            : base(httpClient, apis, scopeProperties, logger, scopedLogger) {

            var tokenResponse = secureTokenService.GetTokenAsync(this).Result;
            HttpClient.SetBearerToken(tokenResponse.AccessToken);
        }


    }
}
