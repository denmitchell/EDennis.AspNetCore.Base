using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.UserLoggerMiddlewareApi.Tests {

    public class TestApis : TestApisBase {
        public override Dictionary<string, Type> EntryPoints =>
            new Dictionary<string, Type> {
                {"UserLoggerApi", typeof(Program) },
            };
    }

}