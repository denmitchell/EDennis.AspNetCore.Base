using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.ApiConfigsApi.Tests {
    public class TestApis : TestApisBase {
        public override Dictionary<string, Type> EntryPoints =>
            new Dictionary<string, Type> {
                {"ApiConfigsApi", typeof(ApiConfigsApi.Program) },
                {"IdentityServer", typeof(IdentityServer.Program) },
                {"Api1", typeof(Api1.Program) },
                {"Api2", typeof(Api2.Program) }
            };

    }
}