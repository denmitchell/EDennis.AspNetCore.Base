using IdentityModel.Client;
using System;

namespace EDennis.AspNetCore.Base.Web
{
    /// <summary>
    /// If Expiration is in the past, then check LastActive
    /// </summary>
    public class CachedToken {
        public TokenResponse TokenResponse { get; set; }
        public DateTime Expiration { get; set; }
    }
}
