using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Testing {


    public class DefaultHttpMessageHandler : DelegatingHandler {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }

    public class ApiLauncherFixture<TStartup> : IDisposable
        where TStartup : class {

        public int Port { get; private set; }
        public IWebHost Host { get; set; }

        public ApiLauncherFixture() {

            Port = PortInspector.GetRandomAvailablePorts(1)[0];
            var classInfo = new ClassInfo<TStartup>();
            var dir = classInfo.ProjectDirectory;

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<TStartup>()
                .UseContentRoot(dir)
                .UseUrls($"http://localhost:{Port}")
                .ConfigureAppConfiguration(options => {
                    options.SetBasePath(dir);
                    options.AddJsonFile("appsettings.Development.json", true);
                    options.AddEnvironmentVariables();
                    options.AddCommandLine(new string[] { "ASPNETCORE_ENVIRONMENT=Development" });
                })
                .Build();

            Task.Run(() => {
                host.WaitForShutdownAsync();
                host.RunAsync();
            });
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if(Host != null)
                        Host.StopAsync();
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        #endregion


    }
}
