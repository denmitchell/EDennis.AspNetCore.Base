using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.AspNetCore.Base {
    public interface IServiceConfig {
        IConfigurationSection ConfigurationSection { get; }
        IServiceCollection Services { get; }

        IServiceConfig Goto(string configKey);
    }
}