using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    public class ScopePropertiesTests : EndpointTests<LauncherFixtureBase> {
        public ScopePropertiesTests(ITestOutputHelper output, 
            LauncherFixtureBase launcherFixture) : base (output,launcherFixture) {
        }
    }
}
