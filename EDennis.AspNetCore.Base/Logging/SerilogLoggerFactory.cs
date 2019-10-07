using Microsoft.Extensions.Logging;
using Serilog.Debugging;
using Serilog.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging
{

    /// <summary>
    /// Factory for producing Serilog Loggers.
    /// From: https://github.com/serilog/serilog-extensions-logging ... src/Serilog.Extensions.Logging/Extensions/Logging/SerilogLoggerFactory.cs 
    /// </summary>
    public class SerilogLoggerFactory : ILoggerFactory
    {
        private readonly SerilogLoggerProvider _provider;

        /// <summary>
        /// Constructs a new logger factory, based upon an existing core logger
        /// </summary>
        /// <param name="logger">Serilog.ILogger instance to use as basis for constructing factory</param>
        /// <param name="dispose">Whether to dispose the logger factory when the provider is disposed (sort of, but not quite)</param>
        public SerilogLoggerFactory(Serilog.ILogger logger = null, bool dispose = false) {
            _provider = new SerilogLoggerProvider(logger, dispose);
        }

        /// <summary>
        /// Disposes of LoggerFactory
        /// </summary>
        public void Dispose() => _provider.Dispose();

        /// <summary>
        /// Creates a new logger with the provided category name
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName) {
            return _provider.CreateLogger(categoryName);
        }

        /// <summary>
        /// Adds a provider ... Note
        /// </summary>
        /// <param name="provider"></param>
        public void AddProvider(ILoggerProvider provider) {
            // Serilog does not allow any other providers
            SelfLog.WriteLine("Logger provider {0} not added.  Serilog does not allow adding other providers", provider);
        }
    }


}

