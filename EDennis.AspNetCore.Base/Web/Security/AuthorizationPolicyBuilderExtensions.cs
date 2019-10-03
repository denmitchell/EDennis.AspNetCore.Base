using EDennis.AspNetCore.Base.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    public static class AuthorizationPolicyBuilderExtensions {

        /// <summary>
        /// Adds a <see cref="PositiveClaimsAuthorizationRequirement"/>
        /// to the current instance.
        /// </summary>
        /// <param name="claimType">The claim type that must be present and, if specified, have one ore more allowed values.</param>
        /// <param name="allowedValues">Values the claim must process one or more of for evaluation to succeed.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthorizationPolicyBuilder RequireClaimPatternMatch(
            this AuthorizationPolicyBuilder builder, string claimType, 
            string pattern, ScopePatternOptions options) {
            if (claimType == null) {
                throw new ArgumentNullException(nameof(claimType));
            }


            builder.Requirements.Add(new ClaimPatternAuthorizationRequirement(claimType, pattern, options));
            return builder;
        }


    }
}
