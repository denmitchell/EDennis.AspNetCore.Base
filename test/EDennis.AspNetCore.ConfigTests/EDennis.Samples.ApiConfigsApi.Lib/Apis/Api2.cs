using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EDennis.Samples.ApiConfigsApi.Apis {
    public class Api2 : SecureApiClient {
        public Api2(IHttpClientFactory httpClientFactory, IOptionsMonitor<AspNetCore.Base.Apis> apis, IScopeProperties scopeProperties, ISecureTokenService secureTokenService, ILogger<Api2> logger, IScopedLogger scopedLogger) : base(httpClientFactory, apis, scopeProperties, secureTokenService, logger, scopedLogger) {
        }
        public string GetName() => "Api2";
    }
}
