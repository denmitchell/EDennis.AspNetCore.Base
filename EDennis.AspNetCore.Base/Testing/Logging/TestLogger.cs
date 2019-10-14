using Microsoft.Extensions.Logging;
using System;
using X = Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class TestLogger : ILogger {

        public virtual ILogger InternalLogger { get; set; }
        public virtual X.ITestOutputHelper TestOutputHelper { get; set; }

        public IDisposable BeginScope<TState>(TState state) {
            return InternalLogger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel) {
            return InternalLogger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            InternalLogger.Log(logLevel, eventId, state, exception, formatter);
            XLog(logLevel, formatter.Invoke(state, exception));
        }

        public virtual void XLog(LogLevel logLevel, string message) {
            if (TestOutputHelper != null)
                TestOutputHelper.WriteLine($"{logLevel}.ToUpper(): {message}");
        }
    }
}