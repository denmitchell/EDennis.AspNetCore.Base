using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EDennis.Samples.MultipleConfigsApi.Services {
    public class MyApiClient : SecureApiClient {
        public MyApiClient(IHttpClientFactory httpClientFactory, IOptionsMonitor<Apis> apis, IScopeProperties scopeProperties, ISecureTokenService sts, ILogger logger, IScopedLogger scopedLogger = null) 
            : base(httpClientFactory, apis, scopeProperties, sts, logger, scopedLogger) {
        }
    }
}
