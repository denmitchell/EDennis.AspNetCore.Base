using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web.Middleware.ApiMiddleware {
    public static partial class IServiceCollectionExtensions {
        public static IServiceCollection AddHeadersToClaims(IServiceCollection services, IConfiguration configuration, string configurationKey) {

            services.Configure<HeadersToClaims>(configuration.GetSection(configurationKey));
            return services;
        }
    }
}
