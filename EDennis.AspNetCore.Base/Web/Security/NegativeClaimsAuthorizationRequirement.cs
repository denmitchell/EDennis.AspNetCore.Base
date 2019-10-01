using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Security
{
    /// <summary>
    /// Implements an <see cref="IAuthorizationHandler"/> and <see cref="IAuthorizationRequirement"/>
    /// which succeeds if no Disallowed claim type values are present and if the claim type itself 
    /// is not Disallowed.
    /// 
    /// NOTE: This is adapted from ... https://github.com/aspnet/Security/blob/master/src/Microsoft.AspNetCore.Authorization/Infrastructure/ClaimsAuthorizationRequirement.cs
    /// 
    /// </summary>
    public class NegativeClaimsAuthorizationRequirement : AuthorizationHandler<NegativeClaimsAuthorizationRequirement>, IAuthorizationRequirement
    {
        /// <summary>
        /// Creates a new instance of <see cref="NegativeClaimsAuthorizationRequirement"/>.
        /// </summary>
        /// <param name="claimType">The claim type that must be absent if no values are provided.</param>
        /// <param name="DisallowedValues">The optional list of claim values, which, if present, 
        /// the claim must NOT match.</param>
        public NegativeClaimsAuthorizationRequirement(string claimType, IEnumerable<string> disallowedValues) {
            if (claimType == null) {
                throw new ArgumentNullException(nameof(claimType));
            }

            ClaimType = claimType;
            DisallowedValues = disallowedValues;
        }

        /// <summary>
        /// Gets the claim type that must be present.
        /// </summary>
        public string ClaimType { get; }

        /// <summary>
        /// Gets the optional list of claim values, which, if present, 
        /// the claim must match.
        /// </summary>
        public IEnumerable<string> DisallowedValues { get; }


        /// <summary>
        /// Makes a decision if authorization is allowed based on the claims requirements specified.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NegativeClaimsAuthorizationRequirement requirement) {
            if (context.User != null) {
                var found = false;

                //if there are no Disallowed values specified, just see if the claim type is present
                if (requirement.DisallowedValues == null || !requirement.DisallowedValues.Any()) {
                    found = context.User.Claims.Any(c => string.Equals(
                        c.Type, requirement.ClaimType, StringComparison.OrdinalIgnoreCase));
                
                //otherwise, see if any of the Disallowed values are present
                } else {
                    found = context.User.Claims.Any(c => string.Equals(
                        c.Type, requirement.ClaimType, StringComparison.OrdinalIgnoreCase)
                            && requirement.DisallowedValues.Contains(c.Value, StringComparer.Ordinal));
                }

                //succeed only if no Disallowed values are present and the claim type itself isn't Disallowed 
                if (!found) {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
