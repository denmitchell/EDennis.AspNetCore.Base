namespace EDennis.AspNetCore.Base {
    public class OidcSettings : OAuthSettings {
        public string ResponseType { get; set; } = "code id_token";
        public bool GetClaimsFromUserInfoEndpoint { get; set; } = true;
        public bool AddOfflineAccess { get; set; } = true;
        public string[] AdditionalScopes { get; set; } = new string[] { };
        public string UserScopePrefix { get; set; } = "user_";
    }
}