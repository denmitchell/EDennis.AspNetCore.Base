using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base {
    public sealed class ScopedLoggingAttribute : OnMethodBoundaryAspect {

        public override void OnEntry(MethodExecutionArgs args) {
            var instance = args.Instance;
            if (typeof(IHasScopedLogger).IsAssignableFrom(instance.GetType())) {
                var logger = (instance as IHasScopedLogger).ScopedLogger;
                if (logger.LogLevel == LogLevel.Trace)
                    logger.LogEntry(args, LogLevel.Trace);
            }
        }

        public override void OnExit(MethodExecutionArgs args) {
            var instance = args.Instance;
            if (typeof(IHasScopedLogger).IsAssignableFrom(instance.GetType())) {
                var logger = (instance as IHasScopedLogger).ScopedLogger;
                if (logger.LogLevel == LogLevel.Trace)
                    logger.LogExit(args, LogLevel.Trace);
            }
        }

        public override void OnException(MethodExecutionArgs args) {
            var instance = args.Instance;
            if (typeof(IHasScopedLogger).IsAssignableFrom(instance.GetType())) {
                var logger = (instance as IHasScopedLogger).ScopedLogger;
                    logger.LogException(args);
            }
        }

    }
}