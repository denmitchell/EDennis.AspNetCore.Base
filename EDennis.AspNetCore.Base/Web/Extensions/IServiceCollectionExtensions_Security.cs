using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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

        /// <summary>
        /// Sets Authentication settings for the application to be protected by IdentityServer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="isUserApp">Used to indicate that this is a Hybrid Flow MVC application when true, and an API without Users when false.</param>
        public static void AddClientAuthenticationAndAuthorizationWithDefaultPolicies(this IServiceCollection services, bool isUserApp = false)
        {
            var provider = services.BuildServiceProvider();
            var config = provider.GetRequiredService<IConfiguration>();
            var env = provider.GetRequiredService<IHostingEnvironment>();

            var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains(env.ApplicationName + ",")).FirstOrDefault();

            string authority = "";

            var apiDict = new Dictionary<string, ApiConfig>();
            config.GetSection("Apis").Bind(apiDict);

            if (!isUserApp) // If this is an api, process original version of method
            {
                //identify Identity Server, which is the only configured API without a secret
                var identityServerApi = apiDict.Where(x => string.IsNullOrEmpty(x.Value.Secret)).FirstOrDefault().Value;
                if (identityServerApi == null)
                    throw new ApplicationException($"AddClientAuthenticationAndAuthorizationWithDefaultPolicies requires the presence of a Apis config entry that is an identity server. No Api having property Secret = null appears in appsettings.{env}.json.");

                authority = identityServerApi.BaseAddress;
                if (authority == "")
                    throw new ApplicationException("Identity Server BaseAddress is null.  If you are using ApiLauncher in a development environment, ensure that the launcher is launched at the beginning of ConfigureServices, and ensure that you call services.AwaitLaunchers().");

                var audience = env.ApplicationName;
                if (audience.EndsWith(".Lib"))
                {
                    audience = audience.Substring(0, audience.Length - 4);
                }
                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = authority;
                        options.RequireHttpsMetadata = false;
                        options.Audience = audience;
                    });
            }

            else // this is a User App, initialize MVC Hybrid Flow Client with .NET Identity Users
            {
                //identify Identity Server, which is the only configured API where userApp is true
                var identityServerApi = apiDict.Where(x => x.Value.IsUserApp == true).FirstOrDefault().Value;
                if (identityServerApi == null)
                    throw new ApplicationException($"AddClientAuthenticationAndAuthorizationWithDefaultPolicies requires the presence of a Apis config entry that is an identity server. No Api having property UserApp=true appears in appsettings.{env}.json.");

                authority = identityServerApi.BaseAddress;
                if (authority == "")
                    throw new ApplicationException("Identity Server BaseAddress is null.");

                var secret = identityServerApi.Secret;
                if (secret == "")
                    throw new ApplicationException("Identity Server Secret is null.");

                var scopes = identityServerApi.Scopes;
                if (!(scopes?.Length > 0))
                    throw new ApplicationException("Identity Server Scopes is null");
                
              
                // Set .NET Identity Options
                services.AddAuthentication(options =>
                {
                   
                    options.DefaultScheme = "Cookies";
                    
                    // DefaultChallengeScheme for .NET Identity is set to "oidc" Identity Server (see .AddOpenIdConnect).
                    // This allows Identity Server to handle the login
                    options.DefaultChallengeScheme = "oidc";

                })
                   //.Net Identity Cookie for this application domain
                   .AddCookie("Cookies")
                   // Identity Server settings
                   .AddOpenIdConnect("oidc",options =>
                   {
                       options.SignInScheme = "Cookies";
                       options.Authority = authority;
                       options.RequireHttpsMetadata = false;
                       options.ClientId = env.ApplicationName;
                       options.ClientSecret = secret;
                       options.ResponseType = "code id_token";
                       options.SaveTokens = false;
                       options.GetClaimsFromUserInfoEndpoint = true;

                       // Add each scope in scopes array
                       for (int i = 0; i < scopes.Length; i++)
                       {
                           options.Scope.Add(scopes[i]);
                       }
                   });
            }
            services.AddAuthorizationWithDefaultPolicies(assembly, isUserApp);

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
        /// Policies are always created to check for the scope claim 
        /// If UserApp is true, policies are also created to check for role claim where the role is {appName}.User
        /// 
        /// NOTE: this method is designed to be used with
        /// <see cref="AddDefaultAuthorizationPolicyConvention"/>
        /// 
        /// </summary>
        /// <param name="services">the service collection</param>
        /// <param name="env">the hosting environment</param>
        public static void AddAuthorizationWithDefaultPolicies(this IServiceCollection services, Assembly assembly, bool isUserApp = false)
        {
            var provider = services.BuildServiceProvider();
            var env = provider.GetRequiredService<IHostingEnvironment>();

            // Whether UserApp is true or false, first create all api-action policies that verify client scope claim
            services.AddAuthorization(options =>
            {
                //create application(api-level) policy
                var applicationName = env.ApplicationName;
                CreateScopePolicy(options, applicationName, applicationName, null, isUserApp);

                var controllers = GetControllerTypes(assembly);
                foreach (var controller in controllers)
                {
                    var controllerPath = applicationName + "." + Regex.Replace(controller.Name, "Controller$", "");
                    
                    //Create action level policies
                    var actions = GetActionMethods(controller);
                    var actionScopes = new List<string>();
                    foreach (var action in actions)
                    {
                        actionScopes.Add(controllerPath + '.' + action.Name);
                    }

                    //create controller-level policy
                    CreateScopePolicy(options, controllerPath, applicationName, actionScopes, isUserApp);

                    // create action-level policies
                    foreach (var action in actions)
                    {
             
                        CreateScopePolicy(options, controllerPath + '.' + action.Name, applicationName, null,isUserApp);
                       
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
        private static void CreateScopePolicy(AuthorizationOptions options, string policyName, string applicationName, List<string> actionScopes, bool addUserRole)
        {
            //add parent and grandparent scopes to cover controller-level
            //and app-level scopes
            var scopes = new List<string>() { policyName };

            //add all action-level scopes to controller-level scope
            if (actionScopes != null)
            {
                scopes.AddRange(actionScopes);
            }

            if (policyName != applicationName)
            {
                var parentScope = policyName.DropLastSegment();
                if (parentScope != null)
                    scopes.Add(parentScope);
                if (parentScope != applicationName)
                {
                    var grandparentScope = parentScope.DropLastSegment();
                    if (grandparentScope != null)
                        scopes.Add(grandparentScope);
                }
            }
            //add the scope policy
            // if userApp, also add the app.User role policy.
            //  Currenty supports one User role named {app}.User with full application access
            options.AddPolicy(policyName, builder =>
            {
                builder.RequireClaim("Scope", scopes);
                if (addUserRole)
                {
                    builder.RequireClaim("role", applicationName+".User");
                }
            });  
        }

        private static string DropLastSegment(this string path)
        {
            int index = path.LastIndexOf(".");
            if (index == -1)
                return null;
            return path.Substring(0, index);
        }



        /// <summary>
        /// Returns a collection of controller types
        /// </summary>
        /// <returns>all controller types</returns>
        private static IEnumerable<Type> GetControllerTypes(Assembly assembly)
        {
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
        private static IEnumerable<MethodInfo> GetActionMethods(Type controllerType)
        {
            var methods = controllerType
                .GetMethods()
                .Where(m => m.GetCustomAttributes(true)
                .Any(h => httpMethodAttributes.Contains(h.GetType())));
            return methods;
        }


    }

}


