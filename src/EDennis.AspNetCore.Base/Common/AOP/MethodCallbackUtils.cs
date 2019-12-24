using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace EDennis.AspNetCore.Base {
    public static class MethodCallbackUtils {

        public static void OnEntry(MethodExecutionArgs args, IScopedLogger scopedLogger) {
            var scope = scopedLogger.Logger.GetScope(args);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            using (scopedLogger.BeginScope(scope)) {
                scopedLogger.Logger.LogTrace("Entering {Method} with Arguments: {Arguments}",
                    method, formatted);
            }
        }

        public static void OnExit(MethodExecutionArgs args, IScopedLogger scopedLogger) {
            var scope = scopedLogger.Logger.GetScope(args);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            using (scopedLogger.BeginScope(scope)) {
                scopedLogger.Logger.LogTrace("Exiting {Method} with Arguments: {Arguments} and ReturnValue: {ReturnValue}",
                    method, formatted,
                JsonSerializer.Serialize(args.ReturnValue)
                );
            }
        }

        public static void OnException(MethodExecutionArgs args, IScopedLogger scopedLogger) {
            scopedLogger.Logger.LogError(args.Exception, "Exception on {Method} with Arguments: {Arguments}",
                $"{args.Method.DeclaringType.Name}.{args.Method}",
                JsonSerializer.Serialize(args.Arguments)
                );
            throw args.Exception;
        }

    }
}
