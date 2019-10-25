using System.Collections.Generic;
using System.Security.Claims;
using EDennis.AspNetCore.Base.Common;

namespace EDennis.AspNetCore.Base {

    //TODO: Use IScopeProperties for dependency injection, rather than ScopeProperties
    public interface IScopeProperties {
        string User { get; set; }
        Claim[] Claims { get; set; }
        HeaderDictionary Headers { get; set; }
        Profiles Profiles { get; set; }
        string ActiveProfile { get; set; }
        TestConfig TestConfig { get; set; }
        Dictionary<string, object> OtherProperties { get; set; }

    }
}