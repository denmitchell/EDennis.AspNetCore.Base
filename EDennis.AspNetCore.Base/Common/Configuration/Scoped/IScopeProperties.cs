using System.Collections.Generic;
using System.Security.Claims;
using EDennis.AspNetCore.Base.Common;
using EDennis.AspNetCore.Base.Web;

namespace EDennis.AspNetCore.Base {

    //TODO: Use IScopeProperties for dependency injection, rather than ScopeProperties
    public interface IScopeProperties {
        string User { get; set; }
        Claim[] Claims { get; set; }
        HeaderDictionary Headers { get; set; }
        bool NewConnection { get; set; }
        Dictionary<string, object> OtherProperties { get; set; }

    }
}