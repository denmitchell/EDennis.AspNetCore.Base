using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.AspNetCore.Base.Testing {
    public class ConfigurationClassFixture : IDisposable {

        public IConfiguration Configuration { get; }

        public ConfigurationClassFixture() {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();
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
            Dispose(true);
        }
        #endregion
    }
}
