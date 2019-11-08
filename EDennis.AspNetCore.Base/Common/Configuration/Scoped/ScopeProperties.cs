using EDennis.AspNetCore.Base.Common;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {
    public class ScopeProperties : IScopeProperties {
        public string User { get; set; }
        public HeaderDictionary Headers { get; set; }
        public Claim[] Claims { get; set; }
        public Dictionary<string, object> OtherProperties { get; set; }
            = new Dictionary<string, object>();
        public bool NewConnection { get; set; }

        public ScopeProperties() { }

    }
}
