using EDennis.AspNetCore.Base.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public static class IServiceConfigExtensions {

        public static ApiConfig AddApi<TClientImplementation>(this ServiceConfig sc, string configKey)
            where TClientImplementation : ApiClient {
            AddApiClientInternal<TClientImplementation, TClientImplementation>(configKey);
            return new ApiConfig( configKey);
        }

        public ApiConfig AddApi<TClientImplementation>()
            where TClientImplementation : ApiClient =>
            AddApi<TClientImplementation>(typeof(TClientImplementation).Name);

        public ApiConfig AddApi<TClientInterface, TClientImplementation>(string configKey)
            where TClientImplementation : ApiClient, TClientInterface
            where TClientInterface : class {
            AddApiClientInternal<TClientInterface, TClientImplementation>(configKey);
            return new ApiConfig(_services, this, _section, configKey);
        }

        public ApiConfig AddApi<TClientInterface, TClientImplementation>(bool traceable = false)
            where TClientImplementation : ApiClient, TClientInterface
            where TClientInterface : class =>
            AddApi<TClientInterface, TClientImplementation>(typeof(TClientImplementation).Name);

        private void AddApiClientInternal<TClientInterface, TClientImplementation>(string configKey)
            where TClientInterface : class
            where TClientImplementation : ApiClient, TClientInterface {

            _services.TryAddScoped<IScopeProperties, ScopeProperties>();
            _services.TryAddScoped<ScopeProperties, ScopeProperties>();

            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TClientImplementation));

            if (isSecureClient) {
                _services.TryAddSingleton<SecureTokenService, SecureTokenService>();
                _services.TryAddScoped<IdentityServerApi>();
            }
            var api = new Api();
            _section.GetSection(configKey).Bind(configKey);
            _services.AddHttpClient<TClientInterface, TClientImplementation>(
                    options => {
                        options.BaseAddress = new Uri(api.MainAddress);
                    }
                );
            _services.AddHttpClient<TClientImplementation, TClientImplementation>(
                    options => {
                        options.BaseAddress = new Uri(api.MainAddress);
                    }
                );
        }


        public DbContextConfig AddDbContext<TContext>(string configKey)

            where TContext : DbContext {
            var configSection = _config.GetSection(configKey);
            _services.Configure<DbContextSettings<TContext>>(configSection);

            var settings = new DbContextSettings<TContext>();
            configSection.Bind(settings);

            _services.AddDbContext<TContext>(builder => {
                DbConnectionManager.ConfigureDbContextOptionsBuilder(builder, settings);
            });

            _services.AddScoped<DbContextOptionsProvider<TContext>>();
            _services.TryAddSingleton<DbConnectionCache<TContext>>();

            return new DbContextConfig(_services, this, _section, configKey);
        }


        public DbContextConfig AddDbContext<TContext>()
            where TContext : DbContext =>
            AddDbContext<TContext>(typeof(TContext).Name);

        public ServiceConfig AddHeadersToClaims(string configKey) {
            _services.Configure<HeadersToClaims>(_config.GetSection(configKey));
            return this;
        }

        public ServiceConfig AddHeadersToClaims() =>
            AddHeadersToClaims("HeadersToClaims");


        public ServiceConfig AddMockClient(string configKey) {
            _services.Configure<ActiveMockClientSettings>(_config.GetSection(configKey));
            _services.TryAddSingleton<SecureTokenService>();
            return this;
        }

        public ServiceConfig AddMockClient() =>
            AddMockClient("MockClient");

        public ServiceConfig AddMockHeaders(string configKey) {
            _services.Configure<MockHeaderSettingsCollection>(_config.GetSection(configKey));
            return this;
        }

        public ServiceConfig AddMockHeaders() =>
            AddMockHeaders("MockHeaders");

        public ServiceConfig AddScopeProperties(string configKey) {
            _services.Configure<ScopePropertiesSettings>(_config.GetSection(configKey));
            _services.AddScoped<IScopeProperties, ScopeProperties>();
            return this;
        }

        public ServiceConfig AddScopeProperties() =>
            AddScopeProperties("ScopeProperties");


    }
}
