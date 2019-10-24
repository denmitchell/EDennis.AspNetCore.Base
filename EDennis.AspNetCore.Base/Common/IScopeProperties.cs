using System.Collections.Generic;
using System.Security.Claims;
using EDennis.AspNetCore.Base.Common;

namespace EDennis.AspNetCore.Base {
    public interface IScopeProperties : IDictionary<string,object>{
        List<Claim> Claims { get; set; }
        HeaderDictionary Headers { get; set; }
        string User { get; set; }
    }
}