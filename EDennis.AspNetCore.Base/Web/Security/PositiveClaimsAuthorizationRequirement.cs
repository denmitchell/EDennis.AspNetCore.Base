using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Security
{
    /// <summary>
    /// Implements an <see cref="IAuthorizationHandler"/> and <see cref="IAuthorizationRequirement"/>
    /// which succeeds if no Allowed claim type values are present and if the claim type itself 
    /// is not Allowed.
    /// 
    /// NOTE: This is adapted from ... https://github.com/aspnet/Security/blob/master/src/Microsoft.AspNetCore.Authorization/Infrastructure/ClaimsAuthorizationRequirement.cs
    /// 
    /// </summary>
    public class PositiveClaimsAuthorizationRequirement : AuthorizationHandler<PositiveClaimsAuthorizationRequirement>, IAuthorizationRequirement
    {
        /// <summary>
        /// Creates a new instance of <see cref="PositiveClaimsAuthorizationRequirement"/>.
        /// </summary>
        /// <param name="claimtypes">The claim type that must be absent if no values are provided.</param>
        /// <param name="AllowedValues">The optional list of claim values, which, if present, 
        /// the claim must NOT match.</param>
        public PositiveClaimsAuthorizationRequirement(string[] claimtypes, IEnumerable<string> allowedValues) {
            if (claimtypes == null) {
                throw new ArgumentNullException(nameof(claimtypes));
            }

            ClaimTypes = claimtypes;
            AllowedValues = allowedValues;
        }

        /// <summary>
        /// Gets the claim types, one or more of which must be present.
        /// </summary>
        public string[] ClaimTypes { get; }

        /// <summary>
        /// Gets the optional list of claim values, which, if present, 
        /// the claim must match.
        /// </summary>
        public IEnumerable<string> AllowedValues { get; }


        /// <summary>
        /// Makes a decision if authorization is allowed based on the claims requirements specified.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PositiveClaimsAuthorizationRequirement requirement) {
            if (context.User != null) {
                var found = false;

                //if there are no Allowed values specified, just see if one of the claim types is present
                if (requirement.AllowedValues == null || !requirement.AllowedValues.Any()) {
                    found = context.User.Claims.Any(c =>
                            ClaimTypes.Contains(c.Type, StringComparer.OrdinalIgnoreCase));

                //otherwise, see if any of the Allowed values are present
                } else {
                    found = context.User.Claims.Any(c => 
                            ClaimTypes.Contains(c.Type, StringComparer.OrdinalIgnoreCase)
                            && requirement.AllowedValues.Contains(c.Value, StringComparer.Ordinal));
                }

                if (found) {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
