using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using M = Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// Base class for custom Serilog loggers that can live simultaneously
    /// in <code>IEnumerable<ILogger<T>></code>, along with the default logger
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CustomSerilogLogger<T> : M.ILogger<T>
    {
        private readonly M.ILogger _logger;

        /// <summary>
        /// Path to the Loggers in appsettings.json or another configuration source
        /// </summary>
        public virtual string LoggersConfigSection { get; } = "Logging:Loggers";

        /// <summary>
        /// Constructs a new logger using the injected logger factory and configuration
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="configuration"></param>
        public CustomSerilogLogger(M.ILoggerFactory factory, IConfiguration configuration) {
            var simpleClassName = GetClassNameWithoutType();

            //create a Serilog.Core logger
            var slogger = new Serilog.LoggerConfiguration()
                    .ReadFrom.Configuration(configuration, $"{LoggersConfigSection}:{simpleClassName}")
                    .CreateLogger(); 

            //create the ILoggerFactory for Serilog from the Serilog.Core logger
            var serilogLoggerFactory = new SerilogLoggerFactory(slogger);
            var category = typeof(T).Name;

            //create the Microsoft.Extensions.Logging.ILogger from the factory
            _logger = serilogLoggerFactory.CreateLogger(category);
        }

        IDisposable M.ILogger.BeginScope<TState>(TState state)
            => _logger.BeginScope(state);

        bool M.ILogger.IsEnabled(M.LogLevel logLevel)
            => _logger.IsEnabled(logLevel);

        void M.ILogger.Log<TState>(M.LogLevel logLevel,
                                 M.EventId eventId,
                                 TState state,
                                 Exception exception,
                                 Func<TState, Exception, string> formatter)
            => _logger.Log(logLevel, eventId, state, exception, formatter);


        /// <summary>
        /// Get the simplified class name
        /// </summary>
        /// <returns></returns>
        private string GetClassNameWithoutType() {
            var name = this.GetType().Name;
            if (!this.GetType().IsGenericType) 
                return name;
            return name.Substring(0, name.IndexOf('`'));
        }

    }
}
