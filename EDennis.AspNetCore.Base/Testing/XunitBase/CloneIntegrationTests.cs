using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class CloneIntegrationTests<TStartup> : IClassFixture<CloneWebApplicationFactory<TStartup>>, IDisposable 
        where TStartup: class {

        protected readonly ITestOutputHelper _output;
        protected readonly string _instanceName;
        protected readonly CloneWebApplicationFactory<TStartup> _factory;
        protected readonly HttpClient _client;

        protected readonly CloneConnections _cloneConnections;
        protected readonly BlockingCollection<int> _cloneIndexPool;
        protected readonly int _cloneIndex;


        public CloneIntegrationTests(ITestOutputHelper output, CloneWebApplicationFactory<TStartup> factory) {
            _output = output;
            _instanceName = Guid.NewGuid().ToString();
            _factory = factory;

            _cloneConnections = factory.CloneConnections;
            _cloneIndexPool = factory.CloneIndexPool;

            _cloneIndex = _cloneIndexPool.Take();

            _client = factory.CreateClient();
            var port = PortInspector.GetRandomAvailablePorts(1)[0];
            _client.BaseAddress = new Uri($"http://localhost:{port}");
            _client.DefaultRequestHeaders.Add(Interceptor.HDR_USE_CLONE, _cloneIndex.ToString());
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    DatabaseCloneManager.ReleaseClones(_cloneConnections, _cloneIndex);
                    _cloneIndexPool.Add(_cloneIndex);
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
