using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class WriteableIntegrationTests<TStartup> : IClassFixture<WebApplicationFactory<TStartup>>, IDisposable 
        where TStartup: class {

        protected readonly WebApplicationFactory<TStartup> _factory;

        protected ITestOutputHelper Output { get; }
        protected HttpClient HttpClient { get; }
        protected string InstanceName { get; }


        public WriteableIntegrationTests(ITestOutputHelper output, WebApplicationFactory<TStartup> factory) {
            _factory = factory;
            Output = output;
            InstanceName = Guid.NewGuid().ToString();
            HttpClient = factory.CreateClient();
            HttpClient.DefaultRequestHeaders.Add(Interceptor.HDR_USE_INMEMORY, InstanceName);
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    HttpClient.SendResetAsync(Interceptor.HDR_DROP_INMEMORY,InstanceName);
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
