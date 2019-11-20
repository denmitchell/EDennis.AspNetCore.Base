using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ScopePropertiesSettings {

        public UserSource[] UserSource { get; set; } = new UserSource[] { Base.UserSource.JwtNameClaim };

        public bool CopyHeaders { get; set; } = true;
        public bool CopyClaims { get; set; } = true;

        public bool AppendHostPath { get; set; } = false;

    }
}
