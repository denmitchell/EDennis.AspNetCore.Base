using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class WriteableEndpointTests<TStartup> : IDisposable
        where TStartup : class {

        protected ITestOutputHelper Output { get; }
        protected HttpClient HttpClient { get; }
        protected string InstanceName { get; }


        public WriteableEndpointTests(ITestOutputHelper output,
                ConfiguringWebApplicationFactory<TStartup> factory) {
            Output = output;
            HttpClient = TestHttpClientFactory.CreateWriteableClient(factory);
            InstanceName = HttpClient.GetInstanceName();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    HttpClient.SendResetAsync(Interceptor.HDR_DROP_INMEMORY, InstanceName);
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
