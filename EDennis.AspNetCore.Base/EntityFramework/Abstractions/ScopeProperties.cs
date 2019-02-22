using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class ScopeProperties {
        public string User { get; set; }
        public Dictionary<string,object> OtherProperties { get; set; }
    }
}
