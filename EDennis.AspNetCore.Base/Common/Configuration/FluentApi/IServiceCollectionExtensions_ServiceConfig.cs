using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.AspNetCore.Base {
    public static partial class IServiceCollectionExtensions_ServiceConfig {

        public static ServiceConfig GetServiceConfig(this IServiceCollection services, IConfiguration config) {
            return new ServiceConfig(services, config);
        }
    }
}
