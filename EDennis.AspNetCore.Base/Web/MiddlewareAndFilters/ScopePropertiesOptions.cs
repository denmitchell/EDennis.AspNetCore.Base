using IdentityModel;
using System;

namespace EDennis.AspNetCore.Base.Web {

    public enum UserSource {
        CLAIMS_PRINCIPAL_IDENTITY_NAME,
        NAME_CLAIM,
        PREFERRED_USERNAME_CLAIM,
        SUBJECT_CLAIM,
        EMAIL_CLAIM,
        PHONE_CLAIM,
        CLIENT_ID_CLAIM,
        CUSTOM_CLAIM,
        SESSION_ID,
        X_USER_HEADER,
    }

    public class ScopePropertiesOptions {
        public UserSource UserSource { get; set; } = UserSource.CLAIMS_PRINCIPAL_IDENTITY_NAME;
        public string UserSourceClaimType { get; set; }
        public string[] StoreHeadersWithPrefixes { get; set; } = new string[] { "X-" };
        public string[] StoreClaimsOfType { get; set; } = new string[] { JwtClaimTypes.Role, JwtClaimTypes.ClientId, JwtClaimTypes.Subject };
    }
}