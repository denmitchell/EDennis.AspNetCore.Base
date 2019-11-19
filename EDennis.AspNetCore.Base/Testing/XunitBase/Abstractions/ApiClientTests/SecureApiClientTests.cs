using EDennis.AspNetCore.Base.Web;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing
{

    public abstract class SecureApiClientTests<TClient> : IClassFixture<SecureApiClientFixture<TClient>>, IDisposable
        where TClient: SecureApiClient {

        protected ITestOutputHelper Output { get; }
        protected string InstanceName { get; }

        protected TClient ApiClient { get; }

        protected SecureApiClientFixture<TClient> Fixture { get; }

        public SecureApiClientTests(ITestOutputHelper output,
               SecureApiClientFixture<TClient> fixture) {

            Output = output;
            Fixture = fixture;
        }

        public void Dispose() {
            Fixture.SendReset();
        }
    }
}

