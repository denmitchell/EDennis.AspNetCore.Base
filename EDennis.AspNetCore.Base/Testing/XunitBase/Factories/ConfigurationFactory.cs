using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;

namespace EDennis.AspNetCore.Base.Testing {

    //TODO: Determine if this is still needed
    public class ConfigurationFactory<TClass> : IDisposable 
        where TClass : class {

        public IConfiguration Configuration { get; }

        public ConfigurationFactory() {

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            Configuration = new ConfigurationBuilder()
                .AddJsonFile(new ManifestEmbeddedFileProvider(typeof(TClass).Assembly), $"appsettings.json", true, true)
                .AddJsonFile(new ManifestEmbeddedFileProvider(typeof(TClass).Assembly), $"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" })
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
