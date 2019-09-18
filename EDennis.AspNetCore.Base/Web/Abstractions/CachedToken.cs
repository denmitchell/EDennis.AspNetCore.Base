using IdentityModel.Client;
using System;

namespace EDennis.AspNetCore.Base.Web.Abstractions
{
    public class CachedToken {
        public TokenResponse TokenResponse { get; set; }
        public DateTime Expiration { get; set; }
    }
}
