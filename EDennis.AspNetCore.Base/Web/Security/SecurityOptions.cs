using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Security
{
    public class SecurityOptions
    {
        public string IdentityServerApiConfigKey { get; set; }
        public bool ClearDefaultInboundClaimTypeMap { get; set; } = true;
        public ScopePolicyOptions ScopePolicyOptions { get; set; }
        public OidcOptions OidcOptions { get; set; }
    }
}
