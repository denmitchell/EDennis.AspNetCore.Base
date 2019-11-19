using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.AspNetCore.Base {
    public static partial class IServiceCollectionExtensions_ServiceConfig {
        public static ServiceConfig GetAppConfig(this IServiceCollection services, IConfiguration config) {
            return new ServiceConfig(services, config);
        }
    }
}
