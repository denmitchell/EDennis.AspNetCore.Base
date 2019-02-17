using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class InMemoryIntegrationTests<TStartup> : IClassFixture<InMemoryWebApplicationFactory<TStartup>>, IDisposable 
        where TStartup: class {

        protected readonly ITestOutputHelper _output;
        protected readonly string _instanceName;
        protected readonly InMemoryWebApplicationFactory<TStartup> _factory;
        protected readonly HttpClient _client;

        public InMemoryIntegrationTests(ITestOutputHelper output, InMemoryWebApplicationFactory<TStartup> factory) {
            _output = output;
            _instanceName = Guid.NewGuid().ToString();
            _factory = factory;
            _client = factory.CreateClient();
            var port = PortInspector.GetRandomAvailablePorts(1)[0];
            _client.BaseAddress = new Uri($"http://localhost:{port}");
            _client.DefaultRequestHeaders.Add(Interceptor.HDR_USE_INMEMORY, _instanceName);
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _client.SendResetAsync(Interceptor.HDR_DROP_INMEMORY,_instanceName);
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
