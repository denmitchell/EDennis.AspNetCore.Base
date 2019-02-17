using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public enum TestDatabaseType {
        Clone,
        InMemory,
        Readonly
    }
    public class TestInfo {
        public TestDatabaseType TestDatabaseType 
            { get; set; } = TestDatabaseType.Clone;
    }
}
