using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web.Middleware.ApiMiddleware {
    public static class IServiceCollectionExtensions_Api {
        public static IApiServiceBuilder AddApi<TApiClient,TStartup>(IServiceCollection services, IConfiguration configuration, string configurationKey) {

            services.Configure<ApiSettings>(configuration.GetSection(configurationKey));

            return new ApiServiceBuilder {
                ServiceCollection = services,
                Configuration = configuration,
                ConfigurationKey = configurationKey
            };
        }
    }
}
