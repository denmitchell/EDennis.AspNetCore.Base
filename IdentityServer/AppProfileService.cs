using EDennis.AspNetCore.Base.Web.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace IdentityServer {
    public class AppProfileService : UserClientClaimsProfileService{
        public string SubjectId { get; set; }
        public string AppName { get; set; }
        public string RoleName { get; set; }

        public override IEnumerable<Claim> GetUserClientClaims(string clientId, string subjectName) {
            var claims = Config.UserClientClaims
                .Where(c=>c.ClientId == clientId && c.Username == subjectName)
                .SelectMany(c=>c.Claims);
            return claims;
        }
    }
}
