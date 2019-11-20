using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.MultipleConfigsApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.ConfigTests {
    public abstract class LauncherFixtureBase : AbstractLauncherFixture {

        public override string[] Arguments => base.Arguments;

        public override int EntryPointPort => throw new NotImplementedException();

        public override Action<string[]> LauncherMain => Program.RunAsync;
    }
}
