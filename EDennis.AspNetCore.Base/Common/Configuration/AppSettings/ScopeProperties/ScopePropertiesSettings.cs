using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ScopePropertiesSettings {

        public const string HOSTPATH_HEADER = "X-HostPath";
        public const string USER_HEADER = "X-User";
        public const string ROLLBACK_HEADER = "X-Testing-Rollback";

        public UserSource UserSource { get; set; } = UserSource.ClaimsPrincipalIdentityName;

        public string[] Headers { get; set; } = new string[] { USER_HEADER, HOSTPATH_HEADER, ROLLBACK_HEADER };
        public string[] ClaimTypes { get; set; } = new string[] { JwtClaimTypes.Role, JwtClaimTypes.ClientId, JwtClaimTypes.Subject };

        public bool AppendHostPath { get; set; } = false;

    }
}
