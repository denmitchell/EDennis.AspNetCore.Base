using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Security {
    public class DefaultPoliciesAuthorizationPolicyProvider : IAuthorizationPolicyProvider {

        private readonly AuthorizationOptions _options = new AuthorizationOptions();
        private Task<AuthorizationPolicy> _cachedDefaultPolicy;


        public DefaultPoliciesAuthorizationPolicyProvider(IConfiguration configuration, ScopePatternOptions options) {

            //***
            //*** Get the DefaultPolicies added to configuration 
            //*** by AddDefaultAuthorizationPolicyConvention
            //***
            List<string> policies = new List<string>();
            configuration.Bind("DefaultPolicies", policies);

            foreach(var policy in policies)
                _options.AddPolicy(policy, builder => {
                    builder.RequireClaimPatternMatch(policy, options);
            });

        }

        /// <summary>
        /// Gets the default authorization policy.
        /// </summary>
        /// <returns>The default authorization policy.</returns>
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() {
            return GetCachedPolicy(ref _cachedDefaultPolicy, _options.DefaultPolicy);
        }


        private Task<AuthorizationPolicy> GetCachedPolicy(ref Task<AuthorizationPolicy> cachedPolicy, AuthorizationPolicy currentPolicy) {
            var local = cachedPolicy;
            if (local == null || local.Result != currentPolicy) {
                cachedPolicy = local = Task.FromResult(currentPolicy);
            }
            return local;
        }

        /// <summary>
        /// Gets a <see cref="AuthorizationPolicy"/> from the given <paramref name="policyName"/>
        /// </summary>
        /// <param name="policyName">The policy name to retrieve.</param>
        /// <returns>The named <see cref="AuthorizationPolicy"/>.</returns>
        public virtual Task<AuthorizationPolicy> GetPolicyAsync(string policyName) {
            // MVC caches policies specifically for this class, so this method MUST return the same policy per
            // policyName for every request or it could allow undesired access. It also must return synchronously.
            // A change to either of these behaviors would require shipping a patch of MVC as well.
            return Task.FromResult(_options.GetPolicy(policyName));
        }
    }
}
