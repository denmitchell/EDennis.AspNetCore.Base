using EDennis.AspNetCore.Base.Web;
using System;
using System.Net.Http;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing
{

    public abstract class WriteableEndpointTests<TStartup> : IDisposable
        where TStartup : class {

        protected ITestOutputHelper Output { get; }
        protected HttpClient HttpClient { get; }
        protected string InstanceName { get; }

        public ConfiguringWebApplicationFactory<TStartup> Factory { get; set; }

        public WriteableEndpointTests(ITestOutputHelper output,
                ConfiguringWebApplicationFactory<TStartup> factory) {
            Output = output;
            HttpClient = TestHttpClientFactory.CreateWriteableClient(factory);
            InstanceName = HttpClient.GetInstanceName();
            this.Factory = factory;
        }

        public WriteableEndpointTests(ITestOutputHelper output,
                ConfiguringWebApplicationFactory<TStartup> factory,
                string[] commandLineOptions) {
            Output = output;
            HttpClient = TestHttpClientFactory.CreateWriteableClient(factory,commandLineOptions);
            InstanceName = HttpClient.GetInstanceName();
            this.Factory = factory;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    HttpClient.SendResetAsync(Interceptor.TESTING_HDR_DROP_INMEMORY, InstanceName);
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
