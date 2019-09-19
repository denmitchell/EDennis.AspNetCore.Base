using System.Collections.Generic;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {
    public interface IScopeProperties {
        Claim[] Claims { get; set; }
        Dictionary<string, object> OtherProperties { get; set; }
        string User { get; set; }
    }
}