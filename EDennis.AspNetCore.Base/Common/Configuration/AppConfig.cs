using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Configuration {

    public static partial class IServiceCollectionExtensions {
        public static AppConfig GetAppConfig(this IServiceCollection services, IConfiguration config) {
            return new AppConfig(services, config);
        }
    }

    public class AppConfig {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _config;
        private IConfigurationSection _section;

        public AppConfig(IServiceCollection services, IConfiguration config) {
            _config = config;
            _services = services;
            _section = config.GetSection("");
        }

        public AppConfig GetSection(string configKey) {
            _section = _config.GetSection(configKey);
            return this;
        }

        public ApiConfig AddApi<TClientImplementation>(string configKey, bool traceable = false)
            where TClientImplementation : ApiClient, IHasILogger {
            AddDependencies<TClientImplementation>();
            AddApiClientInternal<TClientImplementation, TClientImplementation>(configKey, traceable);
            return new ApiConfig(_services, this, _section, configKey);
        }


        public ApiConfig AddApi<TClientImplementation>(bool traceable = false)
            where TClientImplementation : ApiClient, IHasILogger =>
            AddApi<TClientImplementation>(typeof(TClientImplementation).Name, traceable);



        public ApiConfig AddApi<TClientInterface,TClientImplementation>(string configKey, bool traceable = false)
            where TClientImplementation : ApiClient, IHasILogger, TClientInterface
            where TClientInterface : class, IHasILogger {
            AddDependencies<TClientImplementation>();
            AddApiClientInternal<TClientInterface, TClientImplementation>(configKey, traceable);
            return new ApiConfig(_services, this, _section, configKey);
        }


        public ApiConfig AddApi<TClientInterface,TClientImplementation>(bool traceable = false)
            where TClientImplementation : ApiClient, IHasILogger, TClientInterface
            where TClientInterface : class, IHasILogger  =>
            AddApi<TClientInterface,TClientImplementation>(typeof(TClientImplementation).Name, traceable);


        private void AddDependencies<TClientImplementation>()
            where TClientImplementation : class {

            _services.TryAddScoped<IScopeProperties, ScopeProperties>();

            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TClientImplementation));

            if (isSecureClient) {
                _services.TryAddSingleton<SecureTokenService, SecureTokenService>();
                _services.TryAddScoped<IdentityServerApi>();
            }
        }


        private void AddApiClientInternal<TClientInterface, TClientImplementation>(string configKey, bool traceable)
            where TClientInterface : class, IHasILogger
            where TClientImplementation : ApiClient, TClientInterface {

            _services.Configure<Api<TClientImplementation>>(_section.GetSection(configKey));

            if (traceable)
                _services.AddScopedTraceable<TClientInterface, TClientImplementation>();
            else
                _services.AddHttpClient<TClientInterface, TClientImplementation>();
        }




        public DbContextConfig AddDbContext<TContext>(string configKey)

            where TContext : DbContext {
            var configSection = _config.GetSection(configKey);
            _services.Configure<DbContextSettings<TContext>>(configSection);

            var settings = new DbContextSettings<TContext>();
            configSection.Bind(settings);

            _services.AddDbContext<TContext>(builder => { DbConnectionManager.ConfigureDbContextOptionsBuilder(builder, settings); });
            _services.AddScoped<DbContextOptionsProvider<TContext>>();

            _services.TryAddSingleton<DbConnectionCache<TContext>>();

            return new DbContextConfig(_services, this, _section, configKey);

        }

        public DbContextConfig AddDbContext<TContext>()
            where TContext : DbContext =>
            AddDbContext<TContext>(typeof(TContext).Name);

        public AppConfig AddHeadersToClaims(string configKey) {
            _services.Configure<HeadersToClaims>(_config.GetSection(configKey));
            return this;
        }

        public AppConfig AddHeadersToClaims() =>
            AddHeadersToClaims("HeadersToClaims");


        public AppConfig AddMockClient(string configKey) {
            _services.Configure<ActiveMockClientSettings>(_config.GetSection(configKey));
            _services.TryAddSingleton<SecureTokenService>();
            return this;
        }

        public AppConfig AddMockClient() =>
            AddMockClient("MockClient");

        public AppConfig AddMockHeaders(string configKey) {
            _services.Configure<MockHeaderSettingsCollection>(_config.GetSection(configKey));
            return this;
        }

        public AppConfig AddMockHeaders() =>
            AddMockHeaders("MockHeaders");

        public AppConfig AddScopeProperties(string configKey) {
            _services.Configure<ScopePropertiesSettings>(_config.GetSection(configKey));
            _services.AddScoped<IScopeProperties, ScopeProperties>();
            return this;
        }

        public AppConfig AddScopeProperties() =>
            AddScopeProperties("ScopeProperties");

    }





    public class ApiConfig {
        private readonly AppConfig _appConfig;
        private readonly IServiceCollection _services;
        private readonly IConfigurationSection _section;

        public ApiConfig(IServiceCollection services, AppConfig appConfig, IConfigurationSection section, string configKey) {
            _services = services;
            _appConfig = appConfig;
            _section = section.GetSection(configKey);
        }

        public AppConfig AddOAuth(string configKey) {

            var settings = new OAuth();
            _section.GetSection(configKey).Bind(settings);

            if (settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            _services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", opt => {
                    opt.Authority = settings.Authority;
                    opt.RequireHttpsMetadata = settings.RequireHttpsMetadata;
                    opt.Audience = settings.Audience;
                });

            return _appConfig;
        }

        public AppConfig AddOAuth() => AddOAuth("OAuth");


        public AppConfig AddOidc(string configKey) {

            var settings = new Oidc();
            _section.GetSection(configKey).Bind(settings);

            if (settings.ClearDefaultInboundClaimTypeMap)
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            _services
                .AddAuthentication(opt => {
                    opt.DefaultScheme = "Cookies";
                    opt.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", opt => {
                    opt.SignInScheme = "Cookies";
                    opt.Authority = settings.MainAddress;
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
            return _appConfig;
        }

        public AppConfig AddOidc() => AddOidc("Oidc");


    }

    public class DbContextConfig {
        private readonly AppConfig _appConfig;
        private readonly IServiceCollection _services;
        private readonly IConfigurationSection _section;

        public DbContextConfig(IServiceCollection services, AppConfig appConfig, IConfigurationSection section, string configKey) {
            _services = services;
            _appConfig = appConfig;
            _section = section.GetSection(configKey);
        }

        public DbContextConfig AddRepo<TRepo>(bool traceable = false)
            where TRepo : class, IHasILogger, IRepo {
            _services.TryAddScoped<IScopeProperties, ScopeProperties>();
            if (traceable)
                _services.AddScopedTraceable<TRepo>();
            else
                _services.AddScoped<TRepo, TRepo>();

            return this;
        }


        public DbContextConfig AddDbContext<TContext>(string configKey)
            where TContext : DbContext
            => _appConfig.AddDbContext<TContext>(configKey);

        public DbContextConfig AddDbContext<TContext>()
            where TContext : DbContext =>
            AddDbContext<TContext>(typeof(TContext).Name);

        public AppConfig GetSection(string configKey)
            => _appConfig.GetSection(configKey);


    }
}
