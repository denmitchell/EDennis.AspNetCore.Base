using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Security {
    public class CachedAuthorizationOptions {
        public AuthorizationOptions AuthorizationOptions { get; internal set; }
    }
}
