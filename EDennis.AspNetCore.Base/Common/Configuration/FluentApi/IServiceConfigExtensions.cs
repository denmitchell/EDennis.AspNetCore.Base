using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

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

        public const string DEFAULT_SCOPE_PROPERTIES_RELATIVE_PATH = ":ScopeProperties";
        public const string DEFAULT_MOCK_HEADERS_RELATIVE_PATH = ":MockHeaders";
        public const string DEFAULT_MOCK_CLIENT_RELATIVE_PATH = ":MockClient";
        public const string DEFAULT_HEADERS_TO_CLAIMS_RELATIVE_PATH = ":HeadersToClaims";

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
            serviceConfig.Services.AddHttpClient<TClientInterface, TClientImplementation>(
                    options => {
                        options.BaseAddress = new Uri(api.MainAddress);
                    }
                );
            serviceConfig.Services.AddHttpClient<TClientImplementation, TClientImplementation>(
                    options => {
                        options.BaseAddress = new Uri(api.MainAddress);
                    }
                );
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
            AddHeadersToClaims(serviceConfig, DEFAULT_HEADERS_TO_CLAIMS_RELATIVE_PATH);


        public static IServiceConfig AddMockClient(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Goto(path);
            serviceConfig.Services.Configure<ActiveMockClientSettings>(serviceConfig.ConfigurationSection);
            serviceConfig.Services.TryAddSingleton<ISecureTokenService,SecureTokenService>();
            return serviceConfig;
        }

        public static IServiceConfig AddMockClient(this IServiceConfig serviceConfig) =>
            AddMockClient(serviceConfig, DEFAULT_MOCK_CLIENT_RELATIVE_PATH);

        public static IServiceConfig AddMockHeaders(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Goto(path);
            serviceConfig.Services.Configure<MockHeaderSettingsCollection>(serviceConfig.ConfigurationSection);
            return serviceConfig;
        }

        public static IServiceConfig AddMockHeaders(this IServiceConfig serviceConfig) =>
            AddMockHeaders(serviceConfig, DEFAULT_MOCK_HEADERS_RELATIVE_PATH);


        public static IServiceConfig AddScopeProperties(this IServiceConfig serviceConfig, string path) {
            serviceConfig.Goto(path);
            serviceConfig.Services.Configure<ScopePropertiesSettings>(serviceConfig.ConfigurationSection);
            serviceConfig.Services.AddScoped<IScopeProperties, ScopeProperties>();
            return serviceConfig;
        }

        public static IServiceConfig AddScopeProperties(this IServiceConfig serviceConfig) =>
            AddScopeProperties(serviceConfig, DEFAULT_SCOPE_PROPERTIES_RELATIVE_PATH);


    }
}
