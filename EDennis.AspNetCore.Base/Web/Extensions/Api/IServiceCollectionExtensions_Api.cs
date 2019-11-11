using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web.Middleware.ApiMiddleware {
    public static partial class IServiceCollectionExtensions {
        public static IApiSettingsBuilder AddApi<TApiClient,TStartup>(IServiceCollection services, IConfiguration configuration, string configurationKey) {

            services.Configure<ApiSettings>(configuration.GetSection(configurationKey));

            return new ApiSettingsBuilder {
                ServiceCollection = services,
                Configuration = configuration,
                ConfigurationKey = configurationKey
            };
        }
    }
}
