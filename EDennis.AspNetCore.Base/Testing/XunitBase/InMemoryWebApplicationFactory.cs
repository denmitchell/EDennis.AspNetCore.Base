using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Data;

namespace EDennis.AspNetCore.Base.Testing {
    public class InMemoryWebApplicationFactory<TStartup> :
        WebApplicationFactory<TStartup>, IDisposable
        where TStartup : class {


        public InMemoryWebApplicationFactory() { }


        protected override void ConfigureWebHost(IWebHostBuilder builder) {

            builder.ConfigureServices(services => {
                services.AddSingleton(new TestInfo 
                { TestDatabaseType = TestDatabaseType.InMemory });
            });
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    base.Dispose(true);
                }
                disposedValue = true;
            }
        }
        #endregion

    }
}
