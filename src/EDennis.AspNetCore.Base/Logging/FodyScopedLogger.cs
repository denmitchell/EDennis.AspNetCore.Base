using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Logging {
    public class FodyScopedLogger : IScopedLogger {

        private readonly IScopedLoggerAssignments _loggerAssignments;

        public FodyScopedLogger(IScopedLoggerAssignments loggerAssignments) {
            _loggerAssignments = loggerAssignments;
        }

        public ILogger Logger { get => _loggerAssignments.GetLogger(LogLevel); }

        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public IDisposable BeginScope<TState>(TState state) {
            return Logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel) {
            return Logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            Logger.Log(logLevel, eventId, state, exception, formatter);
        }


        public virtual void LogEntry(MethodExecutionArgs args, LogLevel level, IScopeProperties scopeProperties = null) {
            var scope = Logger.GetScope(args);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            var user = scopeProperties?.User ?? "Anonymous";
            var message = "For user {User}, entering {Method} with arguments: {Arguments}";
            using (Logger.BeginScope(scope)) {
                var _ = level switch
                {
                    LogLevel.Trace => LogIt(Logger.LogTrace, message, user, method, formatted),
                    LogLevel.Debug => LogIt(Logger.LogDebug, message, user, method, formatted),
                    LogLevel.Information => LogIt(Logger.LogInformation, message, user, method, formatted),
                    _ => default
                };
            }
        }

        public virtual void LogExit(MethodExecutionArgs args, LogLevel level, IScopeProperties scopeProperties = null) {
            var scope = Logger.GetScope(args);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            var user = scopeProperties?.User ?? "Anonymous";
            var message = "For user {User}, exiting {Method} with arguments: {Arguments}";
            using (Logger.BeginScope(scope)) {
                var _ = level switch
                {
                    LogLevel.Trace => LogIt(Logger.LogTrace, message, user, method, formatted),
                    LogLevel.Debug => LogIt(Logger.LogDebug, message, user, method, formatted),
                    LogLevel.Information => LogIt(Logger.LogInformation, message, user, method, formatted),
                    _ => default
                };
            }
        }


        public virtual void LogException(MethodExecutionArgs args, IScopeProperties scopeProperties = null) {
            var scope = Logger.GetScope(args);
            var method = (args.Method as MethodInfo).GetFriendlyName();
            var user = scopeProperties?.User ?? "Anonymous";
            var message = "For user {User}, failing {Method} with exception: {Message}";
            using (Logger.BeginScope(scope)) {
                Logger.LogError(args.Exception, message, user, method, args.Exception.Message);
            }
        }

        private bool LogIt(Action<string, string[]> action, string message, string user, string method, string args) {
            action(message, new string[] { user, method, args });
            return true;
        }


    }

}

