using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public class FlatExpectedActualList {
        public List<Dictionary<string,object>> Expected { get; set; }
        public List<Dictionary<string, object>> Actual { get; set; }
    }
}
