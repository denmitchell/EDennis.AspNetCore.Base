using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// Note: Add appsettings.json, appsettings.{env}.json, and 
    /// (optionally) appsettings.{Shared}.json to the test project.
    /// If using Visual Studio, add these files as linked files.
    /// </summary>
    public class ConfigurationFixture : IDisposable, IConfigurationFixture {

        public IConfiguration Configuration { get; }

        public ConfigurationFixture() {

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            Configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddJsonFile($"appsettings.Shared.json", true, true)
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
