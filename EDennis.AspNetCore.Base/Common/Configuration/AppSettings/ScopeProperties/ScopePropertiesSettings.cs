using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ScopePropertiesSettings {

        public UserSource UserSource { get; set; } = UserSource.JwtNameClaim;

        public bool AppendHostPath { get; set; } = false;

    }
}
