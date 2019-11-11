using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// Extensions to facilitate configuration of security
    /// </summary>
    public static partial class IServiceCollectionExtensions {


        /// <summary>
        /// NOTE: to avoid creating multiple copies of Configuration, pass in IWebHostEnvironment and IConfiguration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public static void AddAuthentication(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration, string configurationKey) {

            AuthenticationSettings settings = new AuthenticationSettings();
            configuration.GetSection(configurationKey).Bind(settings);
            
            services.Configure<AuthenticationSettings>(configuration.GetSection(configurationKey));

            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.Contains(environment.ApplicationName + ","))
                .FirstOrDefault();


            string authority = settings.IdentityServerBaseAddress;

            if (string.IsNullOrEmpty(authority))
                throw new ApplicationException("IdentityServerApi BaseAddress is null.  If you are using ApiLauncher in a development environment, ensure that the launcher is launched at the beginning of ConfigureServices, and ensure that you call services.AwaitLaunchers().");

            var audience = environment.ApplicationName;

            if (settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // If Oidc isn't configured, use bearer tokens for security
            if (settings.Oidc == null) {
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
                       opt.RequireHttpsMetadata = settings.Oidc.RequireHttpsMetadata;
                       opt.ClientId = audience;
                       opt.ClientSecret = settings.ClientSecret;
                       opt.ResponseType = settings.Oidc.ResponseType;
                       opt.SaveTokens = settings.Oidc.SaveTokens;
                       opt.GetClaimsFromUserInfoEndpoint = settings.Oidc.GetClaimsFromUserInfoEndpoint;

                       var scopes = new List<string>();

                       if (settings.Oidc.OidcScope.AddOfflineAccess)
                           scopes.Add("offline_access");

                       scopes.AddRange(settings.Oidc.OidcScope.AdditionalScopes);

                       for (int i = 0; i < scopes.Count(); i++) {
                           opt.Scope.Add(scopes[i]);
                       }

                   });
            }


        }

    }

}

