using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace EDennis.Samples.ScopedLoggerMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ScopedLoggerController : ControllerBase, IHasMethodCallbacks {

        private readonly ILogger<ScopedLoggerController> _logger;
        private readonly IScopedLogger ScopedLogger;
        private readonly IScopeProperties ScopeProperties;

        public ScopedLoggerController(IScopeProperties scopeProperties, ILogger<ScopedLoggerController> logger,
            IScopedLogger scopedLogger) {
            ScopeProperties = scopeProperties;
            _logger = logger;
            ScopedLogger = scopedLogger;

            _logger.LogInformation("UserLoggerController constructed.");
            ScopedLogger.Logger.LogError("UserLoggerController constructed.");
        }

        [HttpGet]
        [MethodCallback]
        public string Get([FromHeader] string queryString) {
            return ScopedLogger.LogLevel.ToString();
        }


        public void OnEntry(MethodExecutionArgs args) 
            => ScopedLogger.LogEntry(args, ScopedLogger.LogLevel); //MethodCallbackUtils.OnEntry(args, ScopedLogger);
        public void OnExit(MethodExecutionArgs args) 
            => ScopedLogger.LogExit(args, ScopedLogger.LogLevel); //MethodCallbackUtils.OnEntry(args, ScopedLogger);                                                                                                                             //MethodCallbackUtils.OnExit(args, ScopedLogger);
        public void OnException(MethodExecutionArgs args) 
            => ScopedLogger.LogException(args); //MethodCallbackUtils.OnException(args, ScopedLogger); 

    }
}
