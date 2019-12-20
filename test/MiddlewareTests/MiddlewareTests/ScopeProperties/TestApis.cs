using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.ScopePropertiesMiddlewareApi.Tests {

    public class TestApis : Dictionary<string,TestApisBase> {
        public TestApis() {
            Add("A", new TestApisBaseExt("A"));
            Add("B", new TestApisBaseExt("B"));
            Add("C", new TestApisBaseExt("C"));
        }
    }

    public class TestApisBaseExt : TestApisBase {

        public TestApisBaseExt(string env) : base(env) { }
        public override Dictionary<string, Type> EntryPoints =>
            new Dictionary<string, Type> {
                {"ScopePropertiesApi", typeof(Program) },
            };

    }

}