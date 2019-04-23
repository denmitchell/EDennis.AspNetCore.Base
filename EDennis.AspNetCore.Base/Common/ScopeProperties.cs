using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {
    public class ScopeProperties {
        public string User { get; set; }
        public Claim[] Claims { get; set; }
        public Dictionary<string, object> OtherProperties { get; set; }
            = new Dictionary<string, object>();
    }
}
