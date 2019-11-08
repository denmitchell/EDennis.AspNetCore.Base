using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base
{
    public class SecuritySettings
    {
        private OidcSettings _oidc;

        public string ClientSecret { get; set; }
        public string IdentityServerApi { get; set; }
        public int IdentityServerPingFrequency { get; set; } = 5 * 60; // default = every 5 minutes
        public bool ClearDefaultInboundClaimTypeMap { get; set; } = true;
        public ScopePatternSettings ScopePattern { get; set; }
        public OidcSettings Oidc { 
            get {
                return _oidc;
            } set {
                _oidc = value;
                //copy userScopePrefix to main security options to pass to policy requirement handler
                ScopePattern.UserScopePrefix = _oidc?.OidcScope?.UserScopePrefix ?? "_scope";
                ScopePattern.IsOidc = true;
            } 
        }
    }
}
