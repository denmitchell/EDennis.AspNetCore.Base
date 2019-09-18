using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System.Linq;

namespace EDennis.AspNetCore.Base.Logging
{
    public class ConsoleLoggerFactory : ILoggerFactory {

        private static readonly ConfigureNamedOptions<ConsoleLoggerOptions> configureNamedOptions = new ConfigureNamedOptions<ConsoleLoggerOptions>("", null);
        private static readonly OptionsFactory<ConsoleLoggerOptions> optionsFactory = new OptionsFactory<ConsoleLoggerOptions>(new[] { configureNamedOptions },
            Enumerable.Empty<IPostConfigureOptions<ConsoleLoggerOptions>>());
        private static readonly OptionsMonitor<ConsoleLoggerOptions> optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(optionsFactory,
            Enumerable.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(), new OptionsCache<ConsoleLoggerOptions>());

        private readonly ILoggerFactory _factory;

        public ConsoleLoggerFactory(LogLevel logLevel) {

            _factory =
                new LoggerFactory(new[] {
                new ConsoleLoggerProvider(optionsMonitor)
                }, new LoggerFilterOptions {
                    MinLevel = logLevel
                });


        }

        public void AddProvider(ILoggerProvider provider) {
            _factory.AddProvider(provider);
        }

        public ILogger CreateLogger(string categoryName) {
            return _factory.CreateLogger(categoryName);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            _factory.Dispose();
            Dispose(true);
        }
        #endregion

    }
}