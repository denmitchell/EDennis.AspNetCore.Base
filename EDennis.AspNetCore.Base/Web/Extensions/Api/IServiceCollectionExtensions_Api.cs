using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    public static partial class IServiceCollectionExtensions {
        public static IApiSettingsBuilder AddApi<TApiClient,TStartup>(this IServiceCollection services, IConfiguration configuration, string configurationKey) {

            var settings = new ApiSettings();

            //for those services that are configured before DI is configured
            configuration.GetSection(configurationKey).Bind(settings);
            settings.Facade = new ApiSettingsFacade {
                Configuration = configuration,
                ConfigurationKey = configurationKey
            };

            //for those services that access ApiSettings via injection of IOptionsMonitor<ApiSettings>
            services.Configure<ApiSettings>(configuration.GetSection(configurationKey));
            services.PostConfigure<ApiSettings>(options => {
                options.Facade = new ApiSettingsFacade {
                    Configuration = configuration,
                    ConfigurationKey = configurationKey
                };
            });

            return new ApiSettingsBuilder {
                ApiSettings = settings,
                ServiceCollection = services,
                Configuration = configuration,
                ConfigurationKey = configurationKey
            };
        }
    }
}
