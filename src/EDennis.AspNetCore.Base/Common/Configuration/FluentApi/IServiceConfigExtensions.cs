using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace EDennis.AspNetCore.Base {

    /// <summary>
    /// This class provides extension methods for IServiceConfig.  IServiceConfig is
    /// an interface that wraps IServiceCollection and IConfiguration and provides navigation
    /// through the configuration tree.  Extensions on this interface allow for a fluent
    /// experience with configuration.
    /// 
    /// The current set of extension methods provides configuration of the following:
    /// <list type="bullet">
    /// <item>An individual Api</item>
    /// <item>An individual DbContext</item>
    /// <item>ScopeProperties settings (for middleware)</item>
    /// <item>MockHeaders settings (for middleware)</item>
    /// <item>MockClient settings (for middleware)</item>
    /// <item>HeadersToClaims settings (for middleware)</item>
    /// </list>
    /// </summary>
    public static class IServiceConfigExtensions {

        public const string DEFAULT_SCOPE_PROPERTIES_PATH = "ScopeProperties";
        public const string DEFAULT_MOCK_HEADERS_PATH = "MockHeaders";
        public const string DEFAULT_MOCK_CLIENT_PATH = "MockClient";
        public const string DEFAULT_HEADERS_TO_CLAIMS_PATH = "HeadersToClaims";
        public const string DEFAULT_USER_LOGGER_PATH = "UserLogger";


        public static IServiceConfig AddScopedLogger<TScopedLogger>(this IServiceConfig serviceConfig)
            where TScopedLogger : class, IScopedLogger {
            serviceConfig.Services.TryAddScoped<IScopedLogger, TScopedLogger>();
            return serviceConfig;

        }

        public static IServiceConfig AddControllersWithDefaultPolicies(this IServiceConfig serviceConfig, 
            IWebHostEnvironment env, string identityServerApiKey) {

            serviceConfig.Services.AddControllers(options => {
                options.Conventions.Add(new DefaultAuthorizationPolicyConvention(env, serviceConfig.Configuration));
            });

            serviceConfig.Goto(identityServerApiKey);
            var api = new Api();
            serviceConfig.ConfigurationSection.Bind(api);

            serviceConfig.Services.AddSingleton<IAuthorizationPolicyProvider>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultPoliciesAuthorizationPolicyProvider>>();
                return new DefaultPoliciesAuthorizationPolicyProvider(
                    serviceConfig.Configuration, api, logger);
                }
            );
            return serviceConfig;
        }


        public static ApiConfig AddApi<TClientImplementation>(this IServiceConfig serviceConfig, string path)
            where TClientImplementation : ApiClient {
            AddApiClientInternal<TClientImplementation, TClientImplementation>(serviceConfig, path);
            return new ApiConfig( serviceConfig, path);
        }

        public static ApiConfig AddApi<TClientImplementation>(this IServiceConfig serviceConfig)
            where TClientImplementation : ApiClient =>
            AddApi<TClientImplementation>(serviceConfig, typeof(TClientImplementation).Name);

        public static ApiConfig AddApi<TClientInterface, TClientImplementation>(this IServiceConfig serviceConfig, string path)
            where TClientImplementation : ApiClient, TClientInterface
            where TClientInterface : class {
            AddApiClientInternal<TClientInterface, TClientImplementation>(serviceConfig, path);
            return new ApiConfig(serviceConfig, path);
        }

        public static ApiConfig AddApi<TClientInterface, TClientImplementation>(this IServiceConfig serviceConfig)
            where TClientImplementation : ApiClient, TClientInterface
            where TClientInterface : class =>
            AddApi<TClientInterface, TClientImplementation>(serviceConfig, typeof(TClientImplementation).Name);

        private static void AddApiClientInternal<TClientInterface, TClientImplementation>(this IServiceConfig serviceConfig, string path)
            where TClientInterface : class
            where TClientImplementation : ApiClient, TClientInterface {

            serviceConfig.Goto(path);

            serviceConfig.Services.TryAddScoped<IScopeProperties, ScopeProperties>();
            serviceConfig.Services.TryAddScoped<ScopeProperties, ScopeProperties>();

            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TClientImplementation));

            if (isSecureClient) {
                serviceConfig.Services.TryAddSingleton<ISecureTokenService, SecureTokenService>();
                serviceConfig.Services.TryAddScoped<IIdentityServerApi,IdentityServerApi>();
            }
            var api = new Api();
            serviceConfig.ConfigurationSection.Bind(api);

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
        }


        public static DbContextConfig AddDbContext<TContext>(this IServiceConfig serviceConfig, string path)

            where TContext : DbContext {

            serviceConfig.Goto(path);

            serviceConfig.Services.Configure<DbContextSettings<TContext>>(serviceConfig.ConfigurationSection);

            var settings = new DbContextSettings<TContext>();
            serviceConfig.ConfigurationSection.Bind(settings);

            serviceConfig.Services.AddDbContext<TContext>(builder => {
                DbConnectionManager.ConfigureDbContextOptionsBuilder(builder, settings);
            });

            serviceConfig.Services.AddScoped<DbContextOptionsProvider<TContext>>();
            serviceConfig.Services.TryAddSingleton<DbConnectionCache<TContext>>();

            return new DbContextConfig(serviceConfig, path);
        }


        public static DbContextConfig AddDbContext<TContext>(this IServiceConfig serviceConfig)
            where TContext : DbContext =>
            AddDbContext<TContext>(serviceConfig, typeof(TContext).Name);


        public static IServiceConfig AddHeadersToClaims(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Goto(path);
            serviceConfig.Services.Configure<HeadersToClaims>(serviceConfig.ConfigurationSection);
            return serviceConfig;
        }

        public static IServiceConfig AddHeadersToClaims(this IServiceConfig serviceConfig) =>
            AddHeadersToClaims(serviceConfig, DEFAULT_HEADERS_TO_CLAIMS_PATH);


        public static IServiceConfig AddMockClient(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Goto(path);
            serviceConfig.Services.Configure<ActiveMockClientSettings>(serviceConfig.ConfigurationSection);
            serviceConfig.Services.TryAddSingleton<ISecureTokenService,SecureTokenService>();
            return serviceConfig;
        }

        public static IServiceConfig AddMockClient(this IServiceConfig serviceConfig) =>
            AddMockClient(serviceConfig, DEFAULT_MOCK_CLIENT_PATH);

        public static IServiceConfig AddMockHeaders(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Goto(path);
            serviceConfig.Services.Configure<MockHeaderSettingsCollection>(serviceConfig.ConfigurationSection);
            return serviceConfig;
        }

        public static IServiceConfig AddMockHeaders(this IServiceConfig serviceConfig) =>
            AddMockHeaders(serviceConfig, DEFAULT_MOCK_HEADERS_PATH);


        public static IServiceConfig AddScopeProperties(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Goto(path);
            serviceConfig.Services.Configure<ScopePropertiesSettings>(serviceConfig.ConfigurationSection);
            serviceConfig.Services.AddScoped<IScopeProperties, ScopeProperties>();
            return serviceConfig;
        }

        public static IServiceConfig AddScopeProperties(this IServiceConfig serviceConfig) =>
            AddScopeProperties(serviceConfig, DEFAULT_SCOPE_PROPERTIES_PATH);

        public static IServiceConfig AddUserLogger(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Goto(path);
            serviceConfig.Services.Configure<UserLoggerSettings>(serviceConfig.ConfigurationSection);
            return serviceConfig;
        }

        public static IServiceConfig AddUserLogger(this IServiceConfig serviceConfig) =>
            AddUserLogger(serviceConfig, DEFAULT_USER_LOGGER_PATH);


        private static string GetApiKey(Type type) {
            var attr = (ApiAttribute)Attribute.GetCustomAttribute(type, typeof(ApiAttribute));
            if (attr != null)
                return attr.Key;
            else
                return type.Name;
        }

    }
}
