using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;
using I = IdentityServer;
namespace EDennis.Samples.OAuthConfigsApi.Tests {
    public class TestApis : TestApisBase {
        public override Dictionary<string, Type> EntryPoints =>
            new Dictionary<string, Type> {
                {"IdentityServer", typeof(I.Program) },
                {"OAuthApi", typeof(Program) },
            };

    }
}