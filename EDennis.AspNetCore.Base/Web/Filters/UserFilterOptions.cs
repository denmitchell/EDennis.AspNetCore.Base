using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {

    public enum SysUserSource {
        UserClaim,
        RequestHeader,
        UserPrincipleName,
        ExistingScopeProperties
    }

    public class UserFilterOptions {

        /// <summary>
        /// Set of sources to be used for SysUser.  The first
        /// source that is not null is used.
        /// </summary>
        public List<SysUserSource> Sources { get; set; } =
            new List<SysUserSource> {
                SysUserSource.ExistingScopeProperties,
                SysUserSource.RequestHeader,
                SysUserSource.UserClaim,
                SysUserSource.UserPrincipleName
            };

        /// <summary>
        /// Set of user claims to be used for SysUser.  The first
        /// claim that is not null is used.
        /// </summary>
        public List<string> SysUserClaimTypes { get; set; } =
            new List<string> {
                "name",
                "client_name",
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                "email"
            };

        /// <summary>
        /// Set of additional claims to add to ScopeProperties, which is
        /// available in Repos and ApiClients.
        /// </summary>
        public List<string> OtherUserClaimsToAddToScopeProperties { get; set; } =
            new List<string> {
                "client_id"
            };

        /// <summary>
        /// This must be true to propagate header to child APIs
        /// </summary>
        public bool AddXUserHeaderForPropagation { get; set; } = true;

    }

}
