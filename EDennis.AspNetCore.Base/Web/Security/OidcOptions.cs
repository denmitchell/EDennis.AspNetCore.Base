using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Security
{
    public class OidcOptions {
        public string ResponseType { get; set; } = "code id_token";
        public bool RequireHttpsMetadata { get; set; } = true;
        public bool SaveTokens { get; set; } = true;
        public bool GetClaimsFromUserInfoEndpoint { get; set; } = true;
        public OidcScopeOptions OidcScopeOptions { get; set; } = new OidcScopeOptions();
    }

    public class OidcScopeOptions
    {
        public string UserScopePrefix = "user_";
        public bool AddClientId { get; set; } = false;
        public bool AddDefaultPolicies { get; set; } = false;
        public bool AddOfflineAccess { get; set; } = true;
        public string[] AdditionalScopes { get; set; } = new string[] { };

    }
}
