namespace EDennis.AspNetCore.Base {
    public class ScopePropertiesSettings {

        public bool Enabled { get; set; } = false;

        public UserSources UserSources { get; set; }

        public bool CopyHeaders { get; set; } = true;
        public bool CopyClaims { get; set; } = true;

        public bool AppendHostPath { get; set; } = false;
    }
}
