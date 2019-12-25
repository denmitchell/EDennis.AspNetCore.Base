using System;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging {
    public interface IScopedLogger {
        ILogger Logger { get; }
        LogLevel LogLevel { get; }

        IDisposable BeginScope<TState>(TState state);
        bool IsEnabled(LogLevel logLevel);
        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
        void LogEntry(MethodExecutionArgs args, LogLevel level);
        void LogException(MethodExecutionArgs args);
        void LogExit(MethodExecutionArgs args, LogLevel level);
    }
}