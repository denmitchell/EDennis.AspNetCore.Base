using EDennis.AspNetCore.Base.Web.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace IdentityServer {
    public class StaticClaimsProfileService : UserClientClaimsProfileService {
        public override IEnumerable<Claim> GetUserClientClaims(string clientId, string subjectName) {
            var claims = Config.UserClientClaims
                .Where(c => c.ClientId == clientId && c.Username == subjectName)
                .SelectMany(c => c.Claims)
                //add user claims
                .Union(Config.GetUsers()
                    .Where(c => c.Username == subjectName)
                        .SelectMany(u => u.Claims));
            return claims;
        }
    }
}
