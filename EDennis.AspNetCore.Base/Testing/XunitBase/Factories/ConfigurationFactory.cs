using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.AspNetCore.Base.Testing {
    public class ConfigurationFactory<TClass> : IDisposable 
        where TClass : class {

        public IConfiguration Configuration { get; }

        public ConfigurationFactory() {

            var classInfo = new ClassInfo<TClass>();
            var dir = classInfo.ProjectDirectory;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .AddCommandLine(new string[] { "ASPNETCORE_ENVIRONMENT=Development" })
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
