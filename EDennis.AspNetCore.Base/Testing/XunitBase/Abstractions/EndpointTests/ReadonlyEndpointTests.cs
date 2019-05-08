using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class ReadonlyEndpointTests<TStartup> :
            IClassFixture<ConfiguringWebApplicationFactory<TStartup>>
        where TStartup : class {

        protected ITestOutputHelper Output { get; }
        protected HttpClient HttpClient { get; }
        protected string InstanceName { get; }

        public ReadonlyEndpointTests(ITestOutputHelper output,
            ConfiguringWebApplicationFactory<TStartup> factory) {
            Output = output;
            HttpClient = TestHttpClientFactory.CreateReadonlyClient(factory);
            InstanceName = "readonly";
        }

    }
}
