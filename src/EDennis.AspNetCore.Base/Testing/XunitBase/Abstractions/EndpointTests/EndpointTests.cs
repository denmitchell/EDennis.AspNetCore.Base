using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class EndpointTests<TLauncherFixture> 
        : IClassFixture<TLauncherFixture>,
        IDisposable

        where TLauncherFixture : OLDAbstractLauncherFixture {

        protected ITestOutputHelper Output { get; }
        protected TLauncherFixture LauncherFixture { get; }
        protected Apis Apis { get; }

        public EndpointTests(ITestOutputHelper output,
            TLauncherFixture launcherFixture) {
            Output = output;
            LauncherFixture = launcherFixture;
            Apis = new Apis();
            launcherFixture.Configuration.GetSection(ApisConfigKey).Bind(Apis);
        }

        public virtual string ApisConfigKey { get; } = "Apis";

        public void Dispose() {
            MiddlewareUtils.Reset(Apis,LauncherFixture.InstanceName);
        }

    }
}
