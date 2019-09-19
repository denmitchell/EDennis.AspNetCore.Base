using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging
{

    /// <summary>
    /// Provides a wrapper around a regular logger to provide control
    /// over enabling and disabling a logger -- through the contructor
    /// or through Configuration.
    /// 
    /// NOTE: to be a scoped logger, this class should be setup in
    /// dependency injection with a Scoped lifetime.
    /// 
    /// A useful pattern in a using class is the following:
    /// <code>
    /// public ILogger _logger;
    /// /// constructor
    /// public SomeClass(ILogger<SomeClass> regularLogger, ScopedLogger<dynamic> scopedLogger) {
    ///     _logger = scopedLogger.Enabled ? scopedLogger.Logger : regularLogger;
    /// }
    ///</code>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScopedLogger<T> : IScopedLogger<T> {
        public bool Enabled { get; set; } = false;
        public ILogger<T> Logger { get; set; }

        /// <summary>
        /// Constructs a new ScopedLogger for use in escalated logging.
        /// </summary>
        /// <param name="logger">The logger to use with scoped logging</param>
        /// <param name="enabled">Optional parameter used to force enablement of scoped logging</param>
        public ScopedLogger(ILogger<T> logger, bool enabled = false) {

            Logger = logger;

            //enable logger, if enabled with constructor 
            Enabled = enabled;
        }
    }
}
