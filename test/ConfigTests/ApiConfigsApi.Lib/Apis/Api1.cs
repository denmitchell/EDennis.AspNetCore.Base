using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.ApiConfigsApi.Apis {
    public class Api1 : SecureApiClient {

        private readonly Dictionary<string,string> diObjects;
        public Api1(IHttpClientFactory httpClientFactory, IOptionsMonitor<AspNetCore.Base.Apis> apis, IScopeProperties scopeProperties, ISecureTokenService secureTokenService, ILogger<Api1> logger, IScopedLogger scopedLogger) : base(httpClientFactory, apis, scopeProperties, secureTokenService, logger, scopedLogger) {
            diObjects = new Dictionary<string,string>{
                { "ApiName","Api1" },
                { "HttpClientBaseAddress",HttpClient.BaseAddress.ToString() },
                { "ApisCount",apis.CurrentValue.Count.ToString() },
                { "ScopePropertiesUser",ScopeProperties.User },
                { "SecureTokenServiceApplicationName",secureTokenService.ApplicationName },
                { "LoggerLevel",Logger.EnabledAt().ToString() },
                { "ScopedLoggerLevel",scopedLogger.LogLevel.ToString() }
            };

        }
        public Dictionary<string,string> GetObjects() => diObjects;

    }
}
