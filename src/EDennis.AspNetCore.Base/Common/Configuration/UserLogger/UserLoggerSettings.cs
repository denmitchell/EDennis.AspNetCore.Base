using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base {
    public class UserLoggerSettings {

        public readonly static UserSource DEFAULT_USER_SOURCE  = UserSource.JwtNameClaim;

        public UserSource UserSource { get; set; }

    }
}
