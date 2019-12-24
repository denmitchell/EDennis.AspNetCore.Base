using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.UserLoggerMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UserLoggerController : ControllerBase, IHasMethodCallbacks {

        private readonly ILogger<UserLoggerController> _logger;
        private readonly IScopedLogger ScopedLogger;

        public UserLoggerController(ILogger<UserLoggerController> logger,
            IScopedLogger scopedLogger) {
            _logger = logger;
            ScopedLogger = scopedLogger;

            _logger.LogCritical("UserLoggerController constructed.");
            ScopedLogger.Logger.LogError("UserLoggerController constructed.");
        }

        [HttpGet]
        [MethodCallback]
        public string Get() {
            return ScopedLogger.LogLevel.ToString();
        }


        public void OnEntry(MethodExecutionArgs args) => MethodCallbackUtils.OnEntry(args, ScopedLogger);
        public void OnExit(MethodExecutionArgs args) => MethodCallbackUtils.OnExit(args, ScopedLogger);
        public void OnException(MethodExecutionArgs args) => MethodCallbackUtils.OnException(args, ScopedLogger); 

    }
}
