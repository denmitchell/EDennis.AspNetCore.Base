using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing.XunitBase.EndpointTests {
    public abstract class IntegerIdRepoControllerEndpointTests<TEntity, TProgram, TLauncher>
        : LauncherEndpointTests<TProgram, TLauncher>
        where TEntity : class, IHasIntegerId, new()
        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new() {
        public IntegerIdRepoControllerEndpointTests(ITestOutputHelper output, LauncherFixture<TProgram, TLauncher> launcherFixture) : base(output, launcherFixture) {
        }




    }
}