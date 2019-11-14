using System.Collections.Generic;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {

    //TODO: Use IScopeProperties for dependency injection, rather than ScopeProperties
    public interface IScopeProperties {
        int LoggerIndex { get; set; }
        string User { get; set; }
        Claim[] Claims { get; set; }
        HeaderDictionary Headers { get; set; }
        bool NewConnection { get; set; }
        Dictionary<string, object> OtherProperties { get; set; }

    }
}