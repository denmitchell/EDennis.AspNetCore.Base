using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ScopePropertiesSettings {

        public UserSource UserSource { get; set; } = UserSource.ClaimsPrincipalIdentityName;

        public string[] Headers { get; set; } = new string[] { Constants.USER_HEADER, Constants.HOSTPATH_HEADER, Constants.ROLLBACK_HEADER_KEY };
        public string[] ClaimTypes { get; set; } = new string[] { JwtClaimTypes.Role, JwtClaimTypes.ClientId, JwtClaimTypes.Subject };

        public bool AppendHostPath { get; set; } = false;

    }
}
