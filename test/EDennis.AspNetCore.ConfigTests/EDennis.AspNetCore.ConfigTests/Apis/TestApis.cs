using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.ApiConfigsApi.Tests {
    public class TestApis : TestApisBase {
        public override Dictionary<string, Type> EntryPoints =>
            new Dictionary<string, Type> {
                {"Api1", typeof(Program) },
                {"Api2", typeof(Program) }
            };

    }
}