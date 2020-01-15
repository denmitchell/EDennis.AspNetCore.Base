using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace EDennis.AspNetCore.Base {

    /// <summary>
    /// This class provides extension methods for IServiceConfig.  IServiceConfig is
    /// an interface that wraps IServiceCollection and IConfiguration and provides navigation
    /// through the configuration tree.  Extensions on this interface allow for a fluent
    /// experience with configuration.
    /// </summary>
    public static class IServiceConfigExtensions {

        public const string DEFAULT_APIS_PATH = "Apis";
        public const string DEFAULT_DBCONTEXTS_PATH = "DbContexts";
        public const string DEFAULT_SCOPE_PROPERTIES_PATH = "ScopeProperties";
        public const string DEFAULT_SCOPED_CONFIGURATION_PATH = "ScopedConfiguration";
        public const string DEFAULT_MOCK_HEADERS_PATH = "MockHeaders";
        public const string DEFAULT_MOCK_CLIENT_PATH = "MockClient";
        public const string DEFAULT_PK_REWRITER_PATH = "PkRewriter";
        public const string DEFAULT_HEADERS_TO_CLAIMS_PATH = "HeadersToClaims";
        public const string DEFAULT_SCOPED_LOGGER_SETTINGS_PATH = "ScopedLogger";

        public const string DEFAULT_OAUTH_RELATIVE_PATH = "OAuth";
        public const string DEFAULT_OIDC_RELATIVE_PATH = "Oidc";
        public const string OAUTH_SCHEME = "Bearer";
        public const string OIDC_SCHEME = "Cookies";
        public const string OIDC_CHALLENGE_SCHEME = "oidc";



        public static IServiceConfig AddScopedLogger(this IServiceConfig serviceConfig)
            =>   AddScopedLogger(serviceConfig, DEFAULT_SCOPED_LOGGER_SETTINGS_PATH);


        public static IServiceConfig AddScopedLogger(this IServiceConfig serviceConfig, 
            string scopedLoggerSettingsKey){

            serviceConfig.Configure<ScopedLoggerSettings>(scopedLoggerSettingsKey);
            return serviceConfig;
        }


        public static IServiceConfig AddControllersWithDefaultPolicies(this IServiceConfig serviceConfig,
            string appName, string identityServerConfigKey) {

            serviceConfig.Services.AddControllers(options => {
                options.Conventions.Add(new DefaultAuthorizationPolicyConvention(appName, serviceConfig.Configuration));
            });

            var api = new Api();
            serviceConfig.Configuration.GetSection(identityServerConfigKey).Bind(api);

            serviceConfig.Services.AddSingleton<IAuthorizationPolicyProvider>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultPoliciesAuthorizationPolicyProvider>>();
                return new DefaultPoliciesAuthorizationPolicyProvider(
                    serviceConfig.Configuration, api, logger);
            }
            );
            return serviceConfig;
        }


        public static IServiceConfig AddRazorPagesWithDefaultPolicies(this IServiceConfig serviceConfig,
            string appName, string identityServerConfigKey) {

            serviceConfig.Services.AddRazorPages(options => {
                options.Conventions.Add(new DefaultAuthorizationPolicyConvention(appName, serviceConfig.Configuration));
            });

            var api = new Api();
            serviceConfig.Configuration.GetSection(identityServerConfigKey).Bind(api);

            serviceConfig.Services.AddSingleton<IAuthorizationPolicyProvider>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultPoliciesAuthorizationPolicyProvider>>();
                return new DefaultPoliciesAuthorizationPolicyProvider(
                    serviceConfig.Configuration, api, logger);
            }
            );
            return serviceConfig;
        }



        public static IServiceConfig AddApi<TClientImplementation>(this IServiceConfig serviceConfig, string path)
            where TClientImplementation : ApiClient {
            AddApiClientInternal<TClientImplementation, TClientImplementation>(serviceConfig, path);
            return serviceConfig;
        }

        public static IServiceConfig AddApi<TClientImplementation>(this IServiceConfig serviceConfig)
            where TClientImplementation : ApiClient =>
            AddApi<TClientImplementation>(serviceConfig, $"{DEFAULT_APIS_PATH}:{typeof(TClientImplementation).Name}");

        public static IServiceConfig AddApi<TClientInterface, TClientImplementation>(this IServiceConfig serviceConfig, string path)
            where TClientImplementation : ApiClient, TClientInterface
            where TClientInterface : class {
            AddApiClientInternal<TClientInterface, TClientImplementation>(serviceConfig, path);
            return serviceConfig;
        }

        public static IServiceConfig AddApi<TClientInterface, TClientImplementation>(this IServiceConfig serviceConfig)
            where TClientImplementation : ApiClient, TClientInterface
            where TClientInterface : class =>
            AddApi<TClientInterface, TClientImplementation>(serviceConfig, $"{DEFAULT_APIS_PATH}:{typeof(TClientImplementation).Name}");

        private static void AddApiClientInternal<TClientInterface, TClientImplementation>(this IServiceConfig serviceConfig, string path)
            where TClientInterface : class
            where TClientImplementation : ApiClient, TClientInterface {

            if (serviceConfig.GetParentObject<Apis>(path) == null)
                serviceConfig.BindAndConfigure<Apis>(serviceConfig.GetParentPath(path));

            serviceConfig.Services.TryAddScoped<IScopeProperties, ScopeProperties>();
            serviceConfig.Services.TryAddScoped<ScopeProperties, ScopeProperties>();

            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TClientImplementation));

            if (isSecureClient) {
                serviceConfig.Services.TryAddSingleton<ISecureTokenService, SecureTokenService>();
                serviceConfig.Services.TryAddScoped<IIdentityServerApi, IdentityServerApi>();
            }

            Api api = new Api();
            serviceConfig.Configuration.GetSection(path).Bind(api);

            //add named client
            var httpClientName = GetApiKey(typeof(TClientImplementation));
            serviceConfig.Services.AddHttpClient(httpClientName,
                    options => {
                        options.BaseAddress = new Uri(api.MainAddress);
                    }
                );

            //setup DI for ApiClient
            serviceConfig.Services.AddScoped<TClientInterface, TClientImplementation>();
            serviceConfig.Services.AddScoped<TClientImplementation, TClientImplementation>();

            if (api.OAuth != null)
                AddOAuth(serviceConfig, api);

            if (api.Oidc != null)
                AddOidc(serviceConfig, api);

        }



        /// <summary>
        /// Configures an IdentityServer API for OAuth (server to server)
        /// </summary>
        /// <param name="path">the absolute or relative (starting with :) configuration key </param>
        /// <returns>the IServiceConfig object for continued method-chaining configuration</returns>
        private static IServiceConfig AddOAuth(IServiceConfig serviceConfig, Api api) {

            var settings = api.OAuth;

            if (settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            serviceConfig.Services.AddAuthentication(OAUTH_SCHEME)
                .AddJwtBearer(OAUTH_SCHEME, opt => {
                    opt.Authority = settings.Authority;
                    opt.RequireHttpsMetadata = settings.RequireHttpsMetadata;
                    opt.Audience = settings.Audience;
                });

            return serviceConfig;
        }


        /// <summary>
        /// Configures an IdentityServer API for OIDC (browser/user to server)
        /// </summary>
        /// <param name="path">the absolute or relative (starting with :) configuration key </param>
        /// <returns>the IServiceConfig object for continued method-chaining configuration</returns>
        private static IServiceConfig AddOidc(IServiceConfig serviceConfig, Api api) {

            var settings = api.Oidc;

            if (settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            serviceConfig.Services
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
            return serviceConfig;
        }


        public static IServiceConfig AddDbContext<TContext>(this IServiceConfig serviceConfig, string path)

            where TContext : DbContext {

            var settings = serviceConfig.BindAndConfigure<DbContextSettings<TContext>>(path);
            var interceptorSettings = serviceConfig.Bind<DbContextInterceptorSettings<TContext>>($"{path}:Interceptor");
            interceptorSettings.ConnectionString = interceptorSettings.ConnectionString ?? settings.ConnectionString;
            interceptorSettings.DatabaseProvider = (interceptorSettings.DatabaseProvider == DatabaseProvider.Unspecified) ? settings.DatabaseProvider : interceptorSettings.DatabaseProvider;

            serviceConfig.Services.PostConfigure<DbContextSettings<TContext>>(
                options => {
                    options.Interceptor = interceptorSettings;
                }
            );
            serviceConfig.Services.AddSingleton<DbConnectionCache<TContext>>();
            serviceConfig.Services.AddSingleton(
                new StoredProcedureDefs<TContext>(settings)
                );
            serviceConfig.Services.AddScoped<DbContextProvider<TContext>>();

            serviceConfig.Services.AddDbContext<TContext>(builder => {
                DbContextProvider<TContext>.BuildBuilder(builder, settings);
            });

            return serviceConfig;
        }


        public static IServiceConfig AddDbContext<TContext>(this IServiceConfig serviceConfig)
            where TContext : DbContext =>
            AddDbContext<TContext>(serviceConfig, $"{DEFAULT_DBCONTEXTS_PATH}:{typeof(TContext).Name}");


        public static IServiceConfig AddRepo<TRepo>(this IServiceConfig serviceConfig)
            where TRepo : class, IRepo {
            serviceConfig.Services.TryAddScoped<IScopeProperties, ScopeProperties>();
            serviceConfig.Services.TryAddScoped<ScopeProperties, ScopeProperties>();
            serviceConfig.Services.AddScoped<TRepo, TRepo>();
            return serviceConfig;
        }


        public static IServiceConfig AddHeadersToClaims(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Configure<HeadersToClaims>(path);
            return serviceConfig;
        }

        public static IServiceConfig AddHeadersToClaims(this IServiceConfig serviceConfig) =>
            AddHeadersToClaims(serviceConfig, DEFAULT_HEADERS_TO_CLAIMS_PATH);


        public static IServiceConfig AddMockClient(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Configure<ActiveMockClientSettings>(path);
            serviceConfig.Services.TryAddSingleton<ISecureTokenService, SecureTokenService>();
            return serviceConfig;
        }

        public static IServiceConfig AddMockClient(this IServiceConfig serviceConfig) =>
            AddMockClient(serviceConfig, DEFAULT_MOCK_CLIENT_PATH);

        public static IServiceConfig AddMockHeaders(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Configure<MockHeaderSettingsCollection>(path);
            return serviceConfig;
        }

        public static IServiceConfig AddMockHeaders(this IServiceConfig serviceConfig) =>
            AddMockHeaders(serviceConfig, DEFAULT_MOCK_HEADERS_PATH);


        public static IServiceConfig AddPkRewriter(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Services.AddSingleton<PerDeveloperIdCache>();
            serviceConfig.Configure<PkRewriterSettings>(path);
            return serviceConfig;
        }

        public static IServiceConfig AddPkRewriter(this IServiceConfig serviceConfig) =>
            AddPkRewriter(serviceConfig, DEFAULT_PK_REWRITER_PATH);


        public static IServiceConfig AddScopedConfiguration(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Configure<ScopedConfigurationSettings>(path);
            return serviceConfig;
        }

        public static IServiceConfig AddScopedConfiguration(this IServiceConfig serviceConfig) =>
            AddScopedConfiguration(serviceConfig, DEFAULT_SCOPED_CONFIGURATION_PATH);


        public static IServiceConfig AddScopeProperties(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Configure<ScopePropertiesSettings>(path);
            serviceConfig.Services.AddScoped<IScopeProperties, ScopeProperties>();
            return serviceConfig;
        }

        public static IServiceConfig AddScopeProperties(this IServiceConfig serviceConfig) =>
            AddScopeProperties(serviceConfig, DEFAULT_SCOPE_PROPERTIES_PATH);


        private static string GetApiKey(Type type) {
            var attr = (ApiAttribute)Attribute.GetCustomAttribute(type, typeof(ApiAttribute));
            if (attr != null)
                return attr.Key;
            else
                return type.Name;
        }

    }
}
