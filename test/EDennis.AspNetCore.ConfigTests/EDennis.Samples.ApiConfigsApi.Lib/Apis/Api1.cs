using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EDennis.Samples.ApiConfigsApi.Apis {
    public class Api1 : SecureApiClient {
        public Api1(IHttpClientFactory httpClientFactory, IOptionsMonitor<AspNetCore.Base.Apis> apis, IScopeProperties scopeProperties, ISecureTokenService secureTokenService, ILogger logger, IScopedLogger scopedLogger) : base(httpClientFactory, apis, scopeProperties, secureTokenService, logger, scopedLogger) {
        }
    }
}
