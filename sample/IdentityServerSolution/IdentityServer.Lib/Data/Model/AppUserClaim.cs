using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Lib.Data.Model {

    /// <summary>
    /// Model class to hold data that feeds AppUserClaimSpec
    /// </summary>
    public class AppUserClaim {
        public string AppClientId { get; set; }
        public string Username { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
