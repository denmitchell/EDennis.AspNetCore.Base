using Microsoft.Extensions.Logging;
using System;

namespace EDennis.AspNetCore.Base.Web {
    public class ScopedLogger : IScopedLogger {

        private readonly IScopedLoggerAssignments _loggerAssignments;

        public ScopedLogger(IScopedLoggerAssignments loggerAssignments) {
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
            Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
