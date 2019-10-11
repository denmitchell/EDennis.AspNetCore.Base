using EDennis.AspNetCore.Base.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// Extensions to facilitate configuration of security
    /// </summary>
    public static class IServiceCollectionExtensions_Security
    {

        /// <summary>
        /// NOTE: to avoid creating multiple copies of Configuration, pass in IHostingEnvironment and IConfiguration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public static void AddClientAuthenticationAndAuthorizationWithDefaultPolicies(this IServiceCollection services,
            SecurityOptions options = null, IHostingEnvironment environment = null, IConfiguration configuration = null) {

            var settings = options ?? new SecurityOptions();

            IConfiguration config = configuration;
            IHostingEnvironment env = environment;

            if(env == null || config == null) {
                var provider = services.BuildServiceProvider();
                config = provider.GetRequiredService<IConfiguration>();
                env = provider.GetRequiredService<IHostingEnvironment>();
            }

            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.Contains(env.ApplicationName + ","))
                .FirstOrDefault();

            var apiDict = new Dictionary<string, ApiConfig>();
            config.GetSection("Apis").Bind(apiDict);

            var identityServerApiName = GetIdentityServerApiType().Name;

            if (!apiDict.ContainsKey(identityServerApiName))
                throw new ApplicationException("IdentityServerApi is not in Configuration.  \"IdentityServerApi\" (or a subclass) must be present in the Apis section of Configuration");

            var identityServerApiConfig = apiDict[identityServerApiName];


            string authority = identityServerApiConfig.BaseAddress;
            if (string.IsNullOrEmpty(authority))
                throw new ApplicationException("IdentityServerApi BaseAddress is null.  If you are using ApiLauncher in a development environment, ensure that the launcher is launched at the beginning of ConfigureServices, and ensure that you call services.AwaitLaunchers().");

            var audience = env.ApplicationName;
            if (audience.EndsWith(".Lib")) {
                audience = audience.Substring(0, audience.Length - 4);
            }


            if(settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            // If Oidc isn't configured, use bearer tokens for security
            if (settings.OidcOptions == null) {

                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", opt => {
                        opt.Authority = authority;
                        opt.RequireHttpsMetadata = false;
                        opt.Audience = audience;
                    });
            } else {


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
                       opt.RequireHttpsMetadata = settings.OidcOptions.RequireHttpsMetadata;
                       opt.ClientId = audience;
                       opt.ClientSecret = settings.ClientSecret;
                       opt.ResponseType = settings.OidcOptions.ResponseType;
                       opt.SaveTokens = settings.OidcOptions.SaveTokens;
                       opt.GetClaimsFromUserInfoEndpoint = settings.OidcOptions.GetClaimsFromUserInfoEndpoint;

                       var scopes = new List<string>();

                       if (settings.OidcOptions.OidcScopeOptions.AddOfflineAccess)
                           scopes.Add("offline_access");

                       scopes.AddRange(settings.OidcOptions.OidcScopeOptions.AdditionalScopes);

                       for (int i = 0; i < scopes.Count(); i++) {
                           opt.Scope.Add(scopes[i]);
                       }

                   });
            }


        }



        private static Type GetIdentityServerApiType() {
            var serviceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t=> t.IsSubclassOf(typeof(IdentityServerApi)))
                .FirstOrDefault();
            if (serviceType != null)
                return serviceType;
            else
                return typeof(IdentityServerApi);
        }

    }

}


