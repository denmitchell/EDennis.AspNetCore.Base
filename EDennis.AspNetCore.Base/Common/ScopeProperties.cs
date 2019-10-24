using EDennis.AspNetCore.Base.Common;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {
    public class ScopeProperties : Dictionary<string, object>, IScopeProperties {
        public string User { get; set; }
        public HeaderDictionary Headers { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
