using EDennis.AspNetCore.Base.Web;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class FactoryEndpointTests<TTestApis>: IClassFixture<TTestApis> 
        where TTestApis : TestApisBase {

        public abstract string ApiKey { get; }
        public virtual string InstanceName { get; } = Guid.NewGuid().ToString();

        public ITestOutputHelper Output { get; }

        public TTestApis TestApis { get; }
        public HttpClient HttpClient { get; }

        public FactoryEndpointTests(
            TTestApis testApis,
            ITestOutputHelper output) {
            TestApis = testApis;
            Output = output;
            HttpClient = testApis.CreateClient[ApiKey]();
        }



        public void Dispose() {
            SendResetAsync().Wait();
        }

        private async Task SendResetAsync() {
            var clients = TestApis.CreateClient.Values.ToList().Select(f => f.Invoke()).ToArray();
            await MiddlewareUtils.Reset(clients, TestApis.InstanceName);
        }


    }
}
