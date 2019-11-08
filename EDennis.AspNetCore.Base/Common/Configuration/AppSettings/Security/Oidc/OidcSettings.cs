namespace EDennis.AspNetCore.Base {
    public class OidcSettings {
        public string ResponseType { get; set; } = "code id_token";
        public bool RequireHttpsMetadata { get; set; } = true;
        public bool SaveTokens { get; set; } = true;
        public bool GetClaimsFromUserInfoEndpoint { get; set; } = true;
        public OidcScopeSettings OidcScope { get; set; } = new OidcScopeSettings();
    }
}