using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base.Security {
    /// <summary>
    /// Implements an <see cref="IAuthorizationHandler"/> and <see cref="IAuthorizationRequirement"/>
    /// which succeeds if no Allowed claim type values are present and if the claim type itself 
    /// is not Allowed.
    /// 
    /// NOTE: This is adapted from ... https://github.com/aspnet/Security/blob/master/src/Microsoft.AspNetCore.Authorization/Infrastructure/ClaimsAuthorizationRequirement.cs
    /// 
    /// </summary>
    public class ClaimPatternAuthorizationRequirement : AuthorizationHandler<ClaimPatternAuthorizationRequirement>, IAuthorizationRequirement {
        /// <summary>
        /// Creates a new instance of <see cref="ClaimPatternAuthorizationRequirement"/>.
        /// </summary>
        /// <param name="claimType">The claim type that must be absent if no values are provided.</param>
        /// <param name="AllowedValues">The optional list of claim values, which, if present, 
        /// the claim must NOT match.</param>
        public ClaimPatternAuthorizationRequirement(string claimType, 
                string pattern, IOptions<DefaultPoliciesOptions> options = null) {

            if (claimType == null) {
                throw new ArgumentNullException(nameof(claimType));
            }

            ClaimType = claimType;
            Pattern = pattern;
            if (options != null) {
                ScopeClaimType = options.Value.ScopeClaimType;
                NamedClaimPatternsType = options.Value.NamedClaimPatternsType;
                ExclusionPrefix = options.Value.ExclusionPrefix;
            }
        }

        /// <summary>
        /// Gets the claim types, one or more of which must be present.
        /// </summary>
        public string ClaimType { get; }

        /// <summary>
        /// Gets the optional list of claim values, which, if present, 
        /// the claim must match.
        /// </summary>
        public string Pattern { get; }

        public string ScopeClaimType { get; } = "Scope";
        public string NamedClaimPatternsType { get; } = "Role";

        /// <summary>
        /// NOTE: Exclusions are evaluated after all included scopes.
        /// NOTE: When only exclusions are present, application-level scope
        ///       is used as the base from which exclusions are applied.
        /// </summary>
        public string ExclusionPrefix { get; } = "-";

        /// <summary>
        /// NOTE: This can be used to configure roles for users.
        /// </summary>
        public Dictionary<string, List<string>> NamedClaimPatterns { get; }


        /// <summary>
        /// Makes a decision if authorization is allowed based on the claims requirements specified.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ClaimPatternAuthorizationRequirement requirement) {

            if (context.User != null) {

                var found = false;
                var hasPositiveScopes = false;

                if (NamedClaimPatterns != null && NamedClaimPatternsType != null) {

                    var ncp = context.User.Claims.Where(c => c.Type == NamedClaimPatternsType);

                    var inheritedPatterns = NamedClaimPatterns.
                        Where(p => ncp.Any(c => c.Value == p.Key)).SelectMany(p => p.Value);

                    var allPatterns = inheritedPatterns
                        .Union(context.User.Claims
                        .Where(c => c.Type == ScopeClaimType).Select(c => c.Value));


                        foreach (var pattern in allPatterns
                                .Where(p=>!p.StartsWith(ExclusionPrefix))) {

                            hasPositiveScopes = true;

                            if (pattern == requirement.Pattern)
                                found = true;
                            else if (requirement.Pattern.StartsWith(pattern + "."))
                                found = true;
                            else if (Regex.IsMatch(requirement.Pattern, pattern.Replace(".", "\\.").Replace("*", ".*")))
                                found = true;
                        }

                        foreach (var pattern in allPatterns
                                .Where(p => p.StartsWith(ExclusionPrefix))
                                .Select(p=>p.Substring(ExclusionPrefix.Length))) {

                            if (!hasPositiveScopes)
                                found = true;

                            if (pattern == requirement.Pattern)
                                found = false;
                            else if (requirement.Pattern.StartsWith(pattern + "."))
                                found = false;
                            else if (Regex.IsMatch(requirement.Pattern, pattern.Replace(".", "\\.").Replace("*", ".*")))
                                found = false;
                        }

                    }

                if (found) {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}



