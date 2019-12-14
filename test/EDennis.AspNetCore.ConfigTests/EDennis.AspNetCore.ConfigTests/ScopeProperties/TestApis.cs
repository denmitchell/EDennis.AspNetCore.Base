using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.ScopePropertiesConfigsApi.Tests {
    public class TestApis : TestApisBase {
        public override Dictionary<string, Type> EntryPoints =>
            new Dictionary<string, Type> {
                {"ScopePropertiesConfigsApi", typeof(Program) },
            };

    }
}