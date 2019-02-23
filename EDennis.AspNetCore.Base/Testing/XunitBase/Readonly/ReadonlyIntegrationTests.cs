using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class ReadonlyIntegrationTests<TStartup> : IClassFixture<WebApplicationFactory<TStartup>>
        where TStartup: class {

        private WebApplicationFactory<TStartup> _factory;

        protected ITestOutputHelper Output { get; }
        protected HttpClient HttpClient { get; }
        protected string InstanceName { get; } = "readonly";

        public ReadonlyIntegrationTests(ITestOutputHelper output, WebApplicationFactory<TStartup> factory) {
            Output = output;
            _factory = factory;
            HttpClient = factory.CreateClient();
            //needed?
            //var port = PortInspector.GetRandomAvailablePorts(1)[0];
            //_client.BaseAddress = new Uri($"http://localhost:{port}");
            HttpClient.DefaultRequestHeaders.Add(Interceptor.HDR_USE_READONLY, "");
        }

    }
}
