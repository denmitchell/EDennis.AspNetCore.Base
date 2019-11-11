using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    public interface ISecureTokenServiceSettingsBuilder {
        ApiSettings ApiSettings { get; set; }
        SecureTokenServiceSettings SecureTokenServiceSettings { get; set; }
        IServiceCollection ServiceCollection { get; set; }
        IConfiguration Configuration { get; set; }
    }

    public class SecureTokenServiceSettingsBuilder : ISecureTokenServiceSettingsBuilder {
        public ApiSettings ApiSettings { get; set; }
        public SecureTokenServiceSettings SecureTokenServiceSettings { get; set; }
        public IServiceCollection ServiceCollection { get; set; }
        public IConfiguration Configuration { get; set; }

    }

    public static class ISecureTokenServiceSettingsBuilder_Extensions {

        public static IApiSettingsBuilder AddApi<TApiClient, TStartup>(this ISecureTokenServiceSettingsBuilder builder, string configurationKey)
            => builder.ServiceCollection.AddApi<TApiClient, TStartup>(builder.Configuration, configurationKey);


        public static ISecureTokenServiceSettingsBuilder AddOAuth(this ISecureTokenServiceSettingsBuilder builder, IWebHostEnvironment environment, string configurationKey) {
           
            var settings = new OAuthSettings();
            builder.Configuration.GetSection(configurationKey).Bind(settings);

            var audience = settings.Audience ?? environment.ApplicationName;

            if (settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.ServiceCollection.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", opt => {
                    opt.Authority = settings.Authority;
                    opt.RequireHttpsMetadata = settings.RequireHttpsMetadata;
                    opt.Audience = audience;
                });

            return builder;
        }

        public static ISecureTokenServiceSettingsBuilder AddOidc(this ISecureTokenServiceSettingsBuilder builder, IWebHostEnvironment environment, string configurationKey) {

            var settings = new OidcSettings();
            builder.Configuration.GetSection(configurationKey).Bind(settings);


            var audience = settings.Audience ?? environment.ApplicationName;

            if (settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            builder.ServiceCollection
                .AddAuthentication(opt => {
                    opt.DefaultScheme = "Cookies";
                    opt.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", opt => {
                    opt.SignInScheme = "Cookies";
                    opt.Authority = builder.ApiSettings.Facade.BaseAddress;
                    opt.RequireHttpsMetadata = settings.RequireHttpsMetadata;
                    opt.ClientId = audience;
                    opt.ClientSecret = settings.ClientSecret;
                    opt.ResponseType = settings.ResponseType;
                    opt.SaveTokens = settings.SaveTokens;
                    opt.GetClaimsFromUserInfoEndpoint = settings.GetClaimsFromUserInfoEndpoint;
                    var scopes = new List<string>();

                    if (settings.AddOfflineAccess)
                        scopes.Add("offline_access");

                    scopes.AddRange(settings.AdditionalScopes);

                    for (int i = 0; i < scopes.Count(); i++) {
                        opt.Scope.Add(scopes[i]);
                    }

                });

            return builder;
        }
    }

}
