using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web
{

    /// <summary>
    /// Extensions to facilitate configuration of security
    /// </summary>
    public static class IServiceCollectionExtensions_Security
    {


        public static void AddClientAuthenticationAndAuthorizationWithDefaultPolicies(this IServiceCollection services,
            IOptions<SecurityOptions> options = null, bool isUserApp = false) {

            var provider = services.BuildServiceProvider();
            var config = provider.GetRequiredService<IConfiguration>();
            var env = provider.GetRequiredService<IHostingEnvironment>();

            var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains(env.ApplicationName + ",")).FirstOrDefault();

            string authority = "";

            var apiDict = new Dictionary<string, ApiConfig>();
            config.GetSection("Apis").Bind(apiDict);

            ApiConfig identityServerApi;
            if (options?.Value?.IdentityServerApiConfigKey != null)
                identityServerApi = apiDict[options.Value.IdentityServerApiConfigKey];
            else if (apiDict.ContainsKey("IdentityServerClient"))
                identityServerApi = apiDict["IdentityServerClient"];
            else if (apiDict.ContainsKey("IdentityServer"))
                identityServerApi = apiDict["IdentityServer"];
            else
                identityServerApi = apiDict.Where(x => string.IsNullOrEmpty(x.Value.Secret)).FirstOrDefault().Value;


            if (identityServerApi == null)
                throw new ApplicationException($"AddClientAuthenticationAndAuthorizationWithDefaultPolicies requires the presence of a Apis config entry that is an identity server.  No Api config entry existed for 'IdentityServer' or 'IdentityServerClient' in appsettings.{env}.json.");

            authority = identityServerApi.BaseAddress;
            if (authority == "")
                throw new ApplicationException("Identity Server BaseAddress is null.  If you are using ApiLauncher in a development environment, ensure that the launcher is launched at the beginning of ConfigureServices, and ensure that you call services.AwaitLaunchers().");

            var audience = env.ApplicationName;
            if (audience.EndsWith(".Lib")) {
                audience = audience.Substring(0, audience.Length - 4);
            }


            // If this is an api, process original version of method
            if (!isUserApp) {

                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", opt => {
                        opt.Authority = authority;
                        opt.RequireHttpsMetadata = false;
                        opt.Audience = audience;
                    });
            } else {

                var secret = identityServerApi.Secret;
                if (secret == "")
                    throw new ApplicationException("Identity Server Secret is null.");

                var scopes = identityServerApi.Scopes;
                if (!(scopes?.Length > 0))
                    throw new ApplicationException("Identity Server Scopes is null");


                // Set .NET Identity Options
                services.AddAuthentication(opt => {

                    opt.DefaultScheme = "Cookies";

                    // DefaultChallengeScheme for .NET Identity is set to "oidc" Identity Server (see .AddOpenIdConnect).
                    // This allows Identity Server to handle the login
                    opt.DefaultChallengeScheme = "oidc";

                })
                   //.Net Identity Cookie for this application domain
                   .AddCookie("Cookies")
                   // Identity Server settings
                   .AddOpenIdConnect("oidc", opt => {
                       opt.SignInScheme = "Cookies";
                       opt.Authority = authority;
                       opt.RequireHttpsMetadata = false;
                       opt.ClientId = env.ApplicationName;
                       opt.ClientSecret = secret;
                       opt.ResponseType = "code id_token";
                       opt.SaveTokens = true;
                       opt.GetClaimsFromUserInfoEndpoint = true;
                       for (int i = 0; i < scopes.Length; i++) {
                           opt.Scope.Add(scopes[i]);
                       }
                   });
            }

            services.AddAuthorizationWithDefaultPolicies(assembly, options);

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
        public static void AddAuthorizationWithDefaultPolicies(this IServiceCollection services, Assembly assembly,
            IOptions<SecurityOptions> options = null) {

            var scopeClaimType = options?.Value?.ScopeClaimType ?? "Scope";

            var provider = services.BuildServiceProvider();
            var env = provider.GetRequiredService<IHostingEnvironment>();

            services.AddAuthorization(opt => {

                var applicationName = env.ApplicationName;
                var controllers = GetControllerTypes(assembly);

                var policyNames = new List<string>();

                foreach (var controller in controllers) {

                    var controllerPath = applicationName + "." + Regex.Replace(controller.Name, "Controller$", "");
                    var actions = GetActionMethods(controller);

                    foreach (var action in actions) {
                        var policyName = controllerPath + '.' + action.Name;

                        opt.AddPolicy(policyName, builder => {
                            builder.RequireClaimPatternMatch(scopeClaimType, policyName, options);
                        });

                    }
                }




            });


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
                .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(m => !m.IsSpecialName)
                .Union(
                    controllerType
                        .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                            .Where(m => m.GetCustomAttributes(true)
                                .Any(h => httpMethodAttributes.Contains(h.GetType()))
                          )
                 );
            return methods;
        }


    }

}


