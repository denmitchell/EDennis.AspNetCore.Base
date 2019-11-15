using Microsoft.Extensions.Logging;
using System;

namespace EDennis.AspNetCore.Base.Web {
    public interface IScopedLogger {
        ILogger Logger { get; }
        LogLevel LogLevel { get; set; }

        IDisposable BeginScope<TState>(TState state);
        bool IsEnabled(LogLevel logLevel);
        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
    }
}