using IdentityModel;

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

        public const string HOSTPATH_HEADER = "X-HostPath";
        public const string USER_HEADER = "X-User";

        public UserSource UserSource { get; set; } = UserSource.CLAIMS_PRINCIPAL_IDENTITY_NAME;
        public string UserSourceClaimType { get; set; }
        public string StoreHeadersWithPattern { get; set; } = "^X-";
        public bool AppendHostPath { get; set; } = false;
        public string StoreClaimTypesWithPattern { get; set; } = $"^({JwtClaimTypes.Role}|{JwtClaimTypes.ClientId}|{JwtClaimTypes.Subject}$";
    }
}