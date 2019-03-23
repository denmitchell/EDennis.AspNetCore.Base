using System;
using System.Collections.Generic;
using System.Text;
using IdentityModel.Client;

namespace EDennis.AspNetCore.Base.Web.Abstractions {
    public class CachedToken {
        public TokenResponse TokenResponse { get; set; }
        public DateTime Expiration { get; set; }
    }
}
