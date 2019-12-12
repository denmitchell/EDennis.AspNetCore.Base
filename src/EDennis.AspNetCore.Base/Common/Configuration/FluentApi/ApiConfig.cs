using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace EDennis.AspNetCore.Base {

    /// <summary>
    /// Utilizing IServiceConfig, this class provides a fluent API for
    /// configuring an Identity Server API Client
    /// </summary>
    public class ApiConfig {

        public const string DEFAULT_OAUTH_RELATIVE_PATH = "OAuth";
        public const string DEFAULT_OIDC_RELATIVE_PATH = "Oidc";
        public const string OAUTH_SCHEME = "Bearer";
        public const string OIDC_SCHEME = "Cookies";
        public const string OIDC_CHALLENGE_SCHEME = "oidc";

        private readonly IServiceConfig _serviceConfig;

        /// <summary>
        /// Constructs a new ApiConfig object for configuration of an individual API,
        /// including configuration of OAuth or Oidc when the API is an IdentityServer.
        /// </summary>
        /// <param name="serviceConfig">the base object holding services, configuration, and current section</param>
        /// <param name="path">the absolute or relative (starting with :) configuration key </param>
        public ApiConfig(IServiceConfig serviceConfig, string path) {
            _serviceConfig = serviceConfig;
            _serviceConfig.Goto(path);
        }


        /// <summary>
        /// Configures an IdentityServer API for OAuth (server to server)
        /// </summary>
        /// <param name="path">the absolute or relative (starting with :) configuration key </param>
        /// <returns>the IServiceConfig object for continued method-chaining configuration</returns>
        public IServiceConfig AddOAuth(string path) {

            var settings = new OAuth();
            _serviceConfig.Goto(path).ConfigurationSection.Bind(settings);

            if (settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            _serviceConfig.Services.AddAuthentication(OAUTH_SCHEME)
                .AddJwtBearer(OAUTH_SCHEME, opt => {
                    opt.Authority = settings.Authority;
                    opt.RequireHttpsMetadata = settings.RequireHttpsMetadata;
                    opt.Audience = settings.Audience;
                });

            return _serviceConfig;
        }

        /// <summary>
        /// Overload of AddOAuth that uses the default relative configuration key
        /// </summary>
        /// <returns>the IServiceConfig object for continued method-chaining configuration</returns>
        public IServiceConfig AddOAuth() => AddOAuth(DEFAULT_OAUTH_RELATIVE_PATH);


        /// <summary>
        /// Configures an IdentityServer API for OIDC (browser/user to server)
        /// </summary>
        /// <param name="path">the absolute or relative (starting with :) configuration key </param>
        /// <returns>the IServiceConfig object for continued method-chaining configuration</returns>
        public IServiceConfig AddOidc(string configKey) {

            var settings = new Oidc();
            _serviceConfig.Goto(configKey).ConfigurationSection.Bind(settings);

            if (settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            _serviceConfig.Services
                .AddAuthentication(opt => {
                    opt.DefaultScheme = OIDC_SCHEME;
                    opt.DefaultChallengeScheme = OIDC_CHALLENGE_SCHEME;
                })
                .AddCookie(OIDC_SCHEME)
                .AddOpenIdConnect(OIDC_CHALLENGE_SCHEME, opt => {
                    opt.SignInScheme = OIDC_SCHEME;
                    opt.Authority = settings.Authority;
                    opt.RequireHttpsMetadata = settings.RequireHttpsMetadata;
                    opt.ClientId = settings.Audience;
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
            return _serviceConfig;
        }

        /// <summary>
        /// Overload of AddOidc that uses the default relative configuration key
        /// </summary>
        /// <returns>the IServiceConfig object for continued method-chaining configuration</returns>
        public IServiceConfig AddOidc() => AddOidc(DEFAULT_OIDC_RELATIVE_PATH);


        public IServiceConfig Goto(string path) =>
            _serviceConfig.Goto(path);

    }
}
