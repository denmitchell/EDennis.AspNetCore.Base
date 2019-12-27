namespace EDennis.AspNetCore.Base {
    public class ScopePropertiesSettings {

        public bool Enabled { get; set; } = false;

        public readonly static UserSource DEFAULT_USER_SOURCE = UserSource.JwtNameClaim;

        public UserSource UserSource { get; set; } = DEFAULT_USER_SOURCE;

        public bool CopyHeaders { get; set; } = true;
        public bool CopyClaims { get; set; } = true;

        public bool AppendHostPath { get; set; } = false;
    }
}
