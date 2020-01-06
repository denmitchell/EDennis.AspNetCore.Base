using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class LauncherEndpointTests<TProgram,TLauncher> 
        : IClassFixture<LauncherFixture<TProgram, TLauncher>>,
        IDisposable

        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new()
        {

        protected ITestOutputHelper Output { get; }
        protected LauncherFixture<TProgram, TLauncher> LauncherFixture { get; }
        public virtual ProgramBase Program { get; set; }
        protected Apis Apis { get; }
        public virtual string InstanceName { get; } = Guid.NewGuid().ToString();

        public LauncherEndpointTests(ITestOutputHelper output,
            LauncherFixture<TProgram, TLauncher> launcherFixture) {
            Output = output;
            LauncherFixture = launcherFixture;
            Apis = new Apis();
            Program.Configuration.GetSection(Program.ApisConfigurationSection).Bind(Apis);
        }


        public void Dispose() {
            MiddlewareUtils.Reset(Apis,InstanceName);
        }

    }
}
