using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class ReadonlyIntegrationTests<TStartup> : IClassFixture<WebApplicationFactory<TStartup>>
        where TStartup: class {

        protected readonly ITestOutputHelper _output;
        protected readonly string _instanceName;
        protected readonly WebApplicationFactory<TStartup> _factory;
        protected readonly HttpClient _client;

        public ReadonlyIntegrationTests(ITestOutputHelper output, WebApplicationFactory<TStartup> factory) {
            _output = output;
            _factory = factory;
            _client = factory.CreateClient();
            //needed?
            //var port = PortInspector.GetRandomAvailablePorts(1)[0];
            //_client.BaseAddress = new Uri($"http://localhost:{port}");
            _client.DefaultRequestHeaders.Add(Interceptor.HDR_USE_READONLY, Interceptor.DEFAULT_NAMED_INSTANCE);
        }

    }
}
