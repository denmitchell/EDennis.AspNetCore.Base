using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;

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
        }

        [HttpGet]
        public string Get() {
            return GetName("Bob");
        }

        public string GetName(string name) {
            return name;
        }

        public void OnEntry(MethodExecutionArgs args) {
            var scope = ScopedLogger.Logger.GetScope(args);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            using (ScopedLogger.BeginScope(scope)) {
                ScopedLogger.Logger.LogTrace("Entering {Method} with Arguments: {Arguments}",
                    method, formatted);
            }
        }

        public void OnExit(MethodExecutionArgs args) {
            var scope = ScopedLogger.Logger.GetScope(args);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            using (ScopedLogger.BeginScope(scope)) {
                ScopedLogger.Logger.LogTrace("Exiting {Method} with Arguments: {Arguments} and ReturnValue: {ReturnValue}",
                    method, formatted,
                JsonSerializer.Serialize(args.ReturnValue)
                );
            }
        }

        public void OnException(MethodExecutionArgs args) {
            ScopedLogger.Logger.LogError(args.Exception, "Exception on {Method} with Arguments: {Arguments}",
                $"{args.Method.DeclaringType.Name}.{args.Method}",
                JsonSerializer.Serialize(args.Arguments)
                );
            throw args.Exception;
        }
    }
}
