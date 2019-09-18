using System.Net.Http;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing
{

    public abstract class ReadonlyEndpointTests<TStartup>
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


        public ReadonlyEndpointTests(ITestOutputHelper output,
            ConfiguringWebApplicationFactory<TStartup> factory,
            string[] commandLineOptions) {
            Output = output;
            HttpClient = TestHttpClientFactory.CreateReadonlyClient(factory, commandLineOptions);
            InstanceName = "readonly";
        }

    }
}
