using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public class FlatExpectedActual {
        public Dictionary<string,object> Expected { get; set; }
        public Dictionary<string, object> Actual { get; set; }
    }
}
