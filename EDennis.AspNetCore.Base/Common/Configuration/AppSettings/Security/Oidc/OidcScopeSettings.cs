namespace EDennis.AspNetCore.Base {
    public class OidcScopeSettings {
        public string UserScopePrefix = "user_";
        public bool AddOfflineAccess { get; set; } = true;
        public string[] AdditionalScopes { get; set; } = new string[] { };

    }
}