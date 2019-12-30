using EDennis.AspNetCore.Base.Logging.Extensions;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Logging {
    public class FodyScopedLogger : IScopedLogger {

        private readonly IScopedLoggerAssignments _loggerAssignments;
        private readonly IScopeProperties _scopeProperties;
        private readonly IOptionsMonitor<ScopedLoggerSettings> _settings;

        public FodyScopedLogger(IScopeProperties scopeProperties, 
            IScopedLoggerAssignments loggerAssignments,
            IOptionsMonitor<ScopedLoggerSettings> settings) {

            _loggerAssignments = loggerAssignments;
            _scopeProperties = scopeProperties;
            _settings = settings;
            var key = GetKey();
            Logger = _loggerAssignments.GetLogger(key);

        }

        private string GetKey() {
            var source = _settings.CurrentValue.AssignmentKeySource;
            var keyName = _settings.CurrentValue.AssignmentKeyName;
            if (source == AssignmentKeySource.User)
                return _scopeProperties.User ?? "Anonymous";
            else if (source == AssignmentKeySource.Claim)
                return _scopeProperties.Claims.FirstOrDefault(c => c.Type.Equals(keyName, StringComparison.OrdinalIgnoreCase))?.Value;
            else if (source == AssignmentKeySource.Header)
                return _scopeProperties.Headers.FirstOrDefault(c => c.Key.Equals(keyName, StringComparison.OrdinalIgnoreCase)).Value;
            else if (source == AssignmentKeySource.OtherProperty)
                return _scopeProperties.OtherProperties.FirstOrDefault(c => c.Key.Equals(keyName, StringComparison.OrdinalIgnoreCase)).Value.ToString();
            else
                return null;
        }

        public ILogger Logger { get; }
        public LogLevel LogLevel { get => Logger.EnabledAt(); }

        public IDisposable BeginScope<TState>(TState state) {
            return Logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel) {
            return Logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            Logger.Log(logLevel, eventId, state, exception, formatter);
        }


        public virtual void LogEntry(MethodExecutionArgs args, LogLevel level) {
            var scope = Logger.GetScope(args);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            var user = _scopeProperties?.User ?? "Anonymous";
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

        public virtual void LogExit(MethodExecutionArgs args, LogLevel level) {
            var scope = Logger.GetScope(args);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            var user = _scopeProperties?.User ?? "Anonymous";
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


        public virtual void LogException(MethodExecutionArgs args) {
            var scope = Logger.GetScope(args);
            var method = (args.Method as MethodInfo).GetFriendlyName();
            var user = _scopeProperties?.User ?? "Anonymous";
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

