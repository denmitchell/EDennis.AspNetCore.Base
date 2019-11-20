using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class EndpointTests<TLauncherFixture> 
        : IClassFixture<TLauncherFixture>,
        IDisposable

        where TLauncherFixture : AbstractLauncherFixture {

        protected ITestOutputHelper Output { get; }
        protected TLauncherFixture LauncherFixture { get; }
        protected ConfigurationFixture ConfigurationFixture { get; }
        protected Apis Apis { get; }

        public EndpointTests(ITestOutputHelper output,
            TLauncherFixture launcherFixture,
            ConfigurationFixture configurationFixture) {
            Output = output;
            LauncherFixture = launcherFixture;
            ConfigurationFixture = configurationFixture;
            Apis = new Apis();
            configurationFixture.Configuration.GetSection(ApisConfigKey).Bind(Apis);
        }

        public virtual string ApisConfigKey { get; } = "Apis";

        public void Dispose() {
            MiddlewareUtils.Reset(Apis,LauncherFixture.InstanceName);
        }

    }
}
