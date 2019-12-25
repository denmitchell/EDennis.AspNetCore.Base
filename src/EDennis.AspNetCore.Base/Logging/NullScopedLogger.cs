using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Logging {
    public class NullScopedLogger : IScopedLogger {


        public NullScopedLogger() {
        }

        public ILogger Logger { get => NullLogger.Instance; }

        public LogLevel LogLevel { get; } = LogLevel.None;

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
        }

        public virtual void LogExit(MethodExecutionArgs args, LogLevel level) {
        }


        public virtual void LogException(MethodExecutionArgs args) {
        }


    }

}

