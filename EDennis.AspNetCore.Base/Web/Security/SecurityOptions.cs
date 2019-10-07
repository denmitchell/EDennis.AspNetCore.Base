using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Security
{
    public class SecurityOptions
    {
        private OidcOptions _oidcOptions;

        public string ClientSecret { get; set; }
        public string IdentityServerApiConfigKey { get; set; }
        public bool ClearDefaultInboundClaimTypeMap { get; set; } = true;
        public ScopePatternOptions ScopePatternOptions { get; set; }
        public OidcOptions OidcOptions { 
            get {
                return _oidcOptions;
            } set {
                _oidcOptions = value;
                //copy userScopePrefix to main security options to pass to policy requirement handler
                ScopePatternOptions.UserScopePrefix = _oidcOptions?.OidcScopeOptions?.UserScopePrefix ?? "_scope";
                ScopePatternOptions.IsOidc = true;
            } 
        }
    }
}
