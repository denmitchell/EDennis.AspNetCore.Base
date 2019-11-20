using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    public class ConfigurationTests : EndpointTests<LauncherFixture> {
        public ConfigurationTests(ITestOutputHelper output, 
            LauncherFixture launcherFixture) : base (output,launcherFixture) {
        }
    }
}
