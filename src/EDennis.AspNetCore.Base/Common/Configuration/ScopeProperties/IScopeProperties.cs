using System.Collections.Generic;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {

    //TODO: Use IScopeProperties for dependency injection, rather than ScopeProperties
    public interface IScopeProperties {
        string User { get; set; }
        string ScopedTraceLoggerKey { get; set; }
        Claim[] Claims { get; set; }
        HeaderDictionary Headers { get; set; }
        Dictionary<string, object> OtherProperties { get; set; }

    }
}