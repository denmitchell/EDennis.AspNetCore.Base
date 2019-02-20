using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.IO;

namespace EDennis.AspNetCore.Base.Testing {
    public class CloneWebApplicationFactory<TStartup> :
        WebApplicationFactory<TStartup>, IDisposable
        where TStartup : class {


        public BlockingCollection<int> CloneIndexPool { get; }
            = new BlockingCollection<int>();

        public CloneConnections CloneConnections { get; }

        public const int DEFAULT_CLONE_COUNT = 5;


        public CloneWebApplicationFactory() {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();

            var cloneCountStr = config["Testing:DatabaseCloneCount"] ?? DEFAULT_CLONE_COUNT.ToString();
            var cloneCount = int.Parse(cloneCountStr);

            CloneConnections = new CloneConnections();
            CloneConnections.CloneCount = cloneCount;
            DatabaseCloneManager.PopulateCloneConnections(CloneConnections);

            CloneConnections.AutomatedTest = true;

            for (int i = 0; i < cloneCount; i++) {
                CloneIndexPool.Add(i);
            }

        }


        protected override void ConfigureWebHost(IWebHostBuilder builder) {

            builder.ConfigureServices(services => {
                services.AddSingleton(CloneConnections);
                services.AddSingleton(new TestInfo());
            });
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    foreach (var context in CloneConnections.Keys) {
                        foreach (var cxn in CloneConnections[context]) {
                            if (cxn.SqlConnection.State == ConnectionState.Open) {
                                cxn.SqlConnection.Close();
                            }
                        }
                    }
                    base.Dispose(true);
                }
                disposedValue = true;
            }
        }
        #endregion

    }
}
