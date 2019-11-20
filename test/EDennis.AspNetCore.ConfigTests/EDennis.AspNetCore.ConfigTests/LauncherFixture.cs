using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.ConfigTests {
    public class LauncherFixture : AbstractLauncherFixture {
        public override int EntryPointPort => throw new NotImplementedException();

        public override Action<string[]> LauncherMain => throw new NotImplementedException();
    }
}
