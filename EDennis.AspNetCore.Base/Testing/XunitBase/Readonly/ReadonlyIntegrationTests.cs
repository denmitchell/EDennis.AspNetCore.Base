using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class ReadonlyIntegrationTests<TStartup> : 
            IClassFixture<ConfiguringWebApplicationFactory<TStartup>>
        where TStartup: class {

        private ConfiguringWebApplicationFactory<TStartup> _factory;

        protected ITestOutputHelper Output { get; }
        protected HttpClient HttpClient { get; }
        protected string InstanceName { get; } = "readonly";

        public ReadonlyIntegrationTests(ITestOutputHelper output, 
            ConfiguringWebApplicationFactory<TStartup> factory) {
            Output = output;
            _factory = factory;
            HttpClient = factory.CreateClient();
            HttpClient.DefaultRequestHeaders.Add(Interceptor.HDR_USE_READONLY, "");
        }

    }
}
