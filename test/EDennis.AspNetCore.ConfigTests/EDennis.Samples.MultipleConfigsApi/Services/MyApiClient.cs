using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EDennis.Samples.MultipleConfigsApi.Services {
    public class MyApiClient : ApiClient {
        public MyApiClient(IHttpClientFactory httpClientFactory, IOptionsMonitor<Apis> apis, IScopeProperties scopeProperties, ILogger logger, IScopedLogger scopedLogger = null) 
            : base(httpClientFactory, apis, scopeProperties, logger, scopedLogger) {
        }
    }
}
