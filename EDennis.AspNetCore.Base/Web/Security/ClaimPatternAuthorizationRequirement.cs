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
                string pattern, IOptions<SecurityOptions> options) {

            if (claimType == null) {
                throw new ArgumentNullException(nameof(claimType));
            }

            ClaimType = claimType;
            Pattern = pattern;
            if (options != null) {
                ScopeClaimType = options.Value.ScopeClaimType;
                PatternClaimType = options.Value.PatternClaimType;
                NamedPatterns = options.Value.NamedPatterns;
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

        public string ScopeClaimType { get; } = "ScopeXXX";
        public string PatternClaimType { get; } = "RoleXXX";

        /// <summary>
        /// NOTE: Exclusions are evaluated after all included scopes.
        /// NOTE: When only exclusions are present, application-level scope
        ///       is used as the base from which exclusions are applied.
        /// </summary>
        public string ExclusionPrefix { get; } = "-XXX";

        /// <summary>
        /// NOTE: This can be used to configure roles for users.
        /// </summary>
        public Dictionary<string, string[]> NamedPatterns { get; }


        /// <summary>
        /// Makes a decision if authorization is allowed based on the claims requirements specified.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ClaimPatternAuthorizationRequirement requirement) {

            if (context.User != null) {

                var found = false;

                //check Scope first
                var scopePatterns = context.User?.Claims?
                    .Where(c => c.Type.ToLower() == ScopeClaimType.ToLower()).Select(c => c.Value);

                if (scopePatterns != null && scopePatterns.Count() > 0)
                    found = IsMatch(requirement.Pattern, scopePatterns);


                if (!found && NamedPatterns != null && PatternClaimType != null) {

                    var ncp = context.User.Claims
                        .Where(c => c.Type.ToLower() == PatternClaimType.ToLower());

                    var namedPatterns = NamedPatterns.
                        Where(p => ncp.Any(c => c.Value == p.Key));

                    foreach (var namedPattern in namedPatterns) {
                        found = IsMatch(requirement.Pattern, namedPattern.Value);
                        if (found)
                            break;
                    }

                }


                if (found) {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }

        private bool IsMatch(string requirementPattern, IEnumerable<string> testPatterns) {

            var found = false;
            var hasPositiveScopes = false;

            foreach (var pattern in testPatterns
                    .Where(p => !p.StartsWith(ExclusionPrefix))) {

                hasPositiveScopes = true;

                if (pattern.ToLower() == requirementPattern.ToLower())
                    found = true;
                else if (requirementPattern.ToLower().StartsWith(pattern.ToLower() + "."))
                    found = true;
                else if (Regex.IsMatch(requirementPattern, pattern.Replace(".", "\\.").Replace("*", ".*"), RegexOptions.IgnoreCase))
                    found = true;
            }

            foreach (var pattern in testPatterns
                    .Where(p => p.StartsWith(ExclusionPrefix))
                    .Select(p => p.Substring(ExclusionPrefix.Length))) {

                if (!hasPositiveScopes)
                    found = true;

                if (pattern.ToLower() == requirementPattern.ToLower())
                    found = false;
                else if (requirementPattern.ToLower().StartsWith(pattern.ToLower() + "."))
                    found = false;
                else if (Regex.IsMatch(requirementPattern, pattern.Replace(".", "\\.").Replace("*", ".*"), RegexOptions.IgnoreCase))
                    found = false;
            }

            return found;

        }


    }
}



