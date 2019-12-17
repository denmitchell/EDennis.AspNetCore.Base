using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ScopePropertiesSettings {

        public readonly static UserSource DEFAULT_USER_SOURCE = UserSource.JwtNameClaim;

        public UserSource UserSource { get; set; } = DEFAULT_USER_SOURCE;

        public bool CopyHeaders { get; set; } = true;
        public bool CopyClaims { get; set; } = true;

        public bool AppendHostPath { get; set; } = false;

    }
}
