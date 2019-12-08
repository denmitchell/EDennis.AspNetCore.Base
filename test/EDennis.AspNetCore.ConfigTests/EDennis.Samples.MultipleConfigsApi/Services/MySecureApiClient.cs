using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EDennis.Samples.MultipleConfigsApi.Services {
    public class MySecureApiClient : SecureApiClient {
        public MySecureApiClient(IHttpClientFactory httpClientFactory, IOptionsMonitor<Apis> apis, IScopeProperties scopeProperties, ISecureTokenService secureTokenService, ILogger logger, IScopedLogger scopedLogger = null) : base(httpClientFactory, apis, scopeProperties, secureTokenService, logger, scopedLogger) {
        }
    }
}
