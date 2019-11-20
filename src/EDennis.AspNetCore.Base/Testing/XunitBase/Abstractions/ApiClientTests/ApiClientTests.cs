using EDennis.AspNetCore.Base.Web;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing
{

    public abstract class ApiClientTests<TClient> : IClassFixture<ApiClientFixture<TClient>>, IDisposable
        where TClient: ApiClient {

        protected ITestOutputHelper Output { get; }
        protected string InstanceName { get; }

        protected TClient ApiClient { get; }

        protected ApiClientFixture<TClient> Fixture { get; }

        public ApiClientTests(ITestOutputHelper output,
               ApiClientFixture<TClient> fixture) {

            Output = output;
            Fixture = fixture;
        }

        public void Dispose() {
            Fixture.SendReset();
        }
    }
}

