using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base {

    /// <summary>
    /// Provides a convenience class for navigating the configuration tree
    /// while holding onto the service collection.  With extension methods
    /// (typically on IServiceConfig), this class can faciliate building a
    /// fluent API for configuration of an application.
    /// </summary>
    public class ServiceConfig : IServiceConfig {

        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
        public IConfigurationSection ConfigurationSection { get; private set; }

        public ServiceConfig(IServiceCollection services, IConfiguration config) {
            Configuration = config;
            Services = services;
            ConfigurationSection = config.GetSection("");
        }

        public IServiceConfig Goto(string configKey) {

            var key = configKey;
            var origPath = ConfigurationSection.Path;

            //navigate up the config tree (analogous to ../ in file system navigation)
            if (configKey.StartsWith("..:")) {
                while (key.StartsWith("..:")) {
                    try {
                        var path = ConfigurationSection.Path;
                        var parent = path.Substring(0, path.LastIndexOf(":"));
                        ConfigurationSection = Configuration.GetSection(parent);
                        key = key.Substring(3);
                    } catch (Exception ex) {
                        throw new ApplicationException($"Cannot navigate to {configKey} from {origPath}: {ex.Message}");
                    }
                }
                key = ":" + key; //add starting colon to key to indicate relative path
            }

            if(key.StartsWith(":")) //relative path                
                ConfigurationSection = ConfigurationSection.GetSection(configKey);
            else //absolute path
                ConfigurationSection = Configuration.GetSection(configKey);
            return this;
        }

    }
}
