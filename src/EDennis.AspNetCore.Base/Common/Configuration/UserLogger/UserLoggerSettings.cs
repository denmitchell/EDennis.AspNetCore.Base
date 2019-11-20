using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class UserLoggerSettings {
        public UserSource[] UserSource { get; set; } = new UserSource[] { Base.UserSource.JwtNameClaim };
    }
}
