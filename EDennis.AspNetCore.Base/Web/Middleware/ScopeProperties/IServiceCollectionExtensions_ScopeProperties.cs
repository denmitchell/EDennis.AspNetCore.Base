using EDennis.AspNetCore.Base.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.AspNetCore.Base.Web.Middleware.ApiMiddleware {
    public static partial class IServiceCollectionExtensions {
        public static IServiceCollection AddScopeProperties(IServiceCollection services, IConfiguration configuration, string configurationKey) {

            services.Configure<ScopePropertiesSettings>(configuration.GetSection(configurationKey));
            services.AddScoped<IScopeProperties, ScopeProperties>();
            return services;
        }
    }
}
