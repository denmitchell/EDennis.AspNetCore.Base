using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class UserLoggerSettings {

        public readonly static UserSource DEFAULT_USER_SOURCE  = UserSource.JwtNameClaim;

        public UserSource UserSource { get; set; } = DEFAULT_USER_SOURCE;

    }
}
