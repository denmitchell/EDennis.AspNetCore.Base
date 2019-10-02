using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base.Web
{

    /// <summary>
    /// Extensions to facilitate configuration of security
    /// </summary>
    public static class IServiceCollectionExtensions_Security {


        public static void AddClientAuthenticationAndAuthorizationWithDefaultPolicies(this IServiceCollection services) {
            var provider = services.BuildServiceProvider();
            var config = provider.GetRequiredService<IConfiguration>();
            var env = provider.GetRequiredService<IWebHostEnvironment>();

            var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains(env.ApplicationName + ",")).FirstOrDefault();

            string authority = "";

            var apiDict = new Dictionary<string, ApiConfig>();
            config.GetSection("Apis").Bind(apiDict);

            //identify Identity Server, which is the only configured API without a secret
            var identityServerApi = apiDict.Where(x => string.IsNullOrEmpty(x.Value.Secret)).FirstOrDefault().Value;
            if (identityServerApi == null)
                throw new ApplicationException($"AddClientAuthenticationAndAuthorizationWithDefaultPolicies requires the presence of a Apis config entry that is an identity server. No Api having property Secret = null appears in appsettings.{env}.json.");

            authority = identityServerApi.BaseAddress;
            if (authority == "")
                throw new ApplicationException("Identity Server BaseAddress is null.  If you are using ApiLauncher in a development environment, ensure that the launcher is launched at the beginning of ConfigureServices, and ensure that you call services.AwaitLaunchers().");

            var audience = env.ApplicationName;
            if (audience.EndsWith(".Lib")) {
                audience = audience[0..^4];
            }
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options => {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = false;
                    options.Audience = audience;
                });

            services.AddAuthorizationWithDefaultPolicies(assembly);

        }

        /// <summary>
        /// Adds all default application-level, 
        /// controller-level, and action-level policies,
        /// where the policy name is either ...
        /// 
        /// {ApplicationName}
        /// {ApplicationName}.{ControllerName}
        /// {ApplicationName}.{ControllerName}.{ActionName}
        /// 
        /// NOTE: this method is designed to be used with
        /// <see cref="AddDefaultAuthorizationPolicyConvention"/>
        /// 
        /// </summary>
        /// <param name="services">the service collection</param>
        /// <param name="env">the hosting environment</param>
        public static void AddAuthorizationWithDefaultPolicies(this IServiceCollection services, Assembly assembly) {

            var provider = services.BuildServiceProvider();
            var env = provider.GetRequiredService<IWebHostEnvironment>();

            services.AddAuthorization(options => {

                //create application(api-level) policy
                var applicationName = env.ApplicationName;
                CreatePolicy(options, applicationName, applicationName, null);

                var controllers = GetControllerTypes(assembly);
                foreach (var controller in controllers) {

                    var controllerPath = applicationName + "." + Regex.Replace(controller.Name, "Controller$", "");
                    var actions = GetActionMethods(controller);


                    var actionScopes = new List<string>();
                    foreach (var action in actions) {
                        actionScopes.Add(controllerPath + '.' + action.Name);
                    }

                    //create controller-level policy
                    CreatePolicy(options, controllerPath, applicationName, actionScopes);


                    foreach (var action in actions) {
                        CreatePolicy(options, controllerPath + '.' + action.Name, applicationName, null);
                    }
                }
            });


        }


        /// <summary>
        /// Creates a scope-based policy, where the
        /// policy name and scope name are the same.
        /// This method ensures that app-level scopes
        /// and controller-level scopes are inherited.
        /// </summary>
        /// <param name="options">Authorization Options 
        /// used to configure AddAuthorization(...)</param>
        /// <param name="policyName">the name of the policy to add</param>
        private static void CreatePolicy(AuthorizationOptions options, string policyName, string applicationName, List<string> actionScopes) {

            //add parent and grandparent scopes to cover controller-level
            //and app-level scopes
            var scopes = new List<string>() { policyName };

            //add all action-level scopes to controller-level scope
            if (actionScopes != null) {
                scopes.AddRange(actionScopes);
            }

            if (policyName != applicationName) {
                var parentScope = policyName.DropLastSegment();
                if (parentScope != null)
                    scopes.Add(parentScope);
                if (parentScope != applicationName) {
                    var grandparentScope = parentScope.DropLastSegment();
                    if (grandparentScope != null)
                        scopes.Add(grandparentScope);
                }
            }
            //add the policy
            options.AddPolicy(policyName, builder => {
                //builder.RequireClaim("Scope", scopes);
                builder.RequireClaim(new string[] { "Scope", "IncludedScope" }, scopes);
                builder.RejectClaim("ExcludedScope", scopes);
            });

        }

        private static string DropLastSegment(this string path) {
            int index = path.LastIndexOf(".");
            if (index == -1)
                return null;
            return path.Substring(0, index);
        }



        /// <summary>
        /// Returns a collection of controller types
        /// </summary>
        /// <returns>all controller types</returns>
        private static IEnumerable<Type> GetControllerTypes(Assembly assembly) {
            var controllerTypes = assembly.GetTypes()
                .Where(t =>
                    t.IsSubclassOf(typeof(ControllerBase)) ||
                    t.IsSubclassOf(typeof(Controller)) ||
                    t.GetCustomAttribute<ApiControllerAttribute>() != null ||
                    t.GetCustomAttribute<RouteAttribute>() != null
                    );
            return controllerTypes;
        }


        //list of HTTP Method attributes
        static readonly Type[] httpMethodAttributes = new Type[] {
                typeof(HttpGetAttribute),
                typeof(HttpPostAttribute),
                typeof(HttpPutAttribute),
                typeof(HttpPatchAttribute),
                typeof(HttpDeleteAttribute),
                typeof(HttpOptionsAttribute),
                typeof(HttpHeadAttribute) };

        /// <summary>
        /// returns a collection of action methods
        /// </summary>
        /// <param name="controllerType">the controller</param>
        /// <returns>all action methods associated with the indicated controller</returns>
        private static IEnumerable<MethodInfo> GetActionMethods(Type controllerType) {
            var methods = controllerType
                .GetMethods()
                .Where(m => m.GetCustomAttributes(true)
                .Any(h => httpMethodAttributes.Contains(h.GetType())));
            return methods;
        }


    }

}


