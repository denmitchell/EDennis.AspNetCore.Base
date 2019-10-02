using EDennis.AspNetCore.Base.Security;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="PositiveClaimsAuthorizationRequirement"/>
        /// to the current instance.
        /// </summary>
        /// <param name="claimType">The claim type that must be absent or have no disallowed values.</param>
        /// <param name="disallowedValues">Values the claim must NOT process one or more of for evaluation to succeed.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthorizationPolicyBuilder RejectClaim(this AuthorizationPolicyBuilder builder, string claimType, IEnumerable<string> disallowedValues) {
            if (claimType == null) {
                throw new ArgumentNullException(nameof(claimType));
            }

            builder.Requirements.Add(new NegativeClaimsAuthorizationRequirement(claimType, disallowedValues));
            return builder;
        }

        /// <summary>
        /// Adds a <see cref="PositiveClaimsAuthorizationRequirement"/>
        /// to the current instance.
        /// </summary>
        /// <param name="claimType">The claim type that must be present and, if specified, have one ore more allowed values.</param>
        /// <param name="allowedValues">Values the claim must process one or more of for evaluation to succeed.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthorizationPolicyBuilder RequireClaim(this AuthorizationPolicyBuilder builder, string[] claimTypes, IEnumerable<string> disallowedValues) {
            if (claimTypes == null) {
                throw new ArgumentNullException(nameof(claimTypes));
            }

            builder.Requirements.Add(new PositiveClaimsAuthorizationRequirement(claimTypes, disallowedValues));
            return builder;
        }


    }
}
