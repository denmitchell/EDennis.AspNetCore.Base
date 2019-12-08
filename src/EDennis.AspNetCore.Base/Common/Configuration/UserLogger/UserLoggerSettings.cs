using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class UserLoggerSettings {
        public HashSet<UserSource> UserSource { get; set; } = new HashSet<UserSource> { Base.UserSource.JwtNameClaim };
    }
}
