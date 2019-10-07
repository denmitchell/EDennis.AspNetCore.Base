using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging
{
    public static class ILoggerExtensions
    {
        /// <summary>
        /// Returns the minimum log level for a given logger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static LogLevel EnabledAt<T>(this ILogger<T> logger) {
            for (int i = (int)LogLevel.Trace; i < (int)LogLevel.None; i++)
                if (logger.IsEnabled((LogLevel)i))
                    return (LogLevel)i;

            return LogLevel.None;
        }

        /// <summary>
        /// Returns the minimum log level for a given logger
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static LogLevel EnabledAt(this ILogger logger) {
            for (int i = (int)LogLevel.Trace; i < (int)LogLevel.None; i++)
                if (logger.IsEnabled((LogLevel)i))
                    return (LogLevel)i;

            return LogLevel.None;
        }
    }
}
