using EDennis.AspNetCore.Base.Common;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {
    public class ScopeProperties : IScopeProperties {
        public string User { get; set; }
        public HeaderDictionary Headers { get; set; }
        public Claim[] Claims { get; set; }
        public Profiles Profiles { get; set; }
        public string ActiveProfile { get; set; } = "Default";
        public TestConfig TestConfig {get; set;}
        public Dictionary<string, object> OtherProperties { get; set; }
            = new Dictionary<string, object>();
        public ScopeProperties(IOptionsMonitor<Profiles> profiles) {
            Profiles = profiles.CurrentValue;
        }

    }
}
