using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.ScopedLoggerMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    [ScopedTraceLogger(logEntry:true,logExit:true)]
    public class ScopedLoggerController : ControllerBase {

        private readonly ILogger<ScopedLoggerController> _logger;
        
        private IScopeProperties _scopeProperties;

        [DisableWeaving]
        public string GetScopedTraceLoggerKey() => _scopeProperties?.ScopedTraceLoggerKey;



        public ScopedLoggerController(IScopeProperties scopeProperties, ILogger<ScopedLoggerController> logger) {
            _scopeProperties = scopeProperties;
            _logger = logger;

            _logger.LogInformation("ScopedLoggerController constructed.");
        }

        [HttpGet]
        public string Get([FromHeader] string queryString) {
            var key = GetScopedTraceLoggerKey();
            if (ScopedTraceLoggerAttribute.RegisteredKeys.ContainsKey(key ?? ""))
                return GetLevel();
            else
                return LogLevel.None.ToString();
        }

        
        public string GetLevel() {
            return LogLevel.Trace.ToString();
        }

    }
}
