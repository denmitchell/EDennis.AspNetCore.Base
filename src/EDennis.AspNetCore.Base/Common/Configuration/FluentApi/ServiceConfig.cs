using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

        /// <summary>
        /// Navigate configuration tree
        ///     .. or ..: means navigate up to parent node
        ///     ..:..: means navigate up to grandparent node
        ///     : or "" means navigate to root
        /// </summary>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public IServiceConfig Goto(string configKey) {

            var key = configKey;
            var origPath = ConfigurationSection.Path;

            if (configKey.Equals(":") || configKey.Equals("")) {
                ConfigurationSection = Configuration.GetSection("");
                return this;
            }

            //navigate up the config tree (analogous to ../ in file system navigation)
            while (key.StartsWith("..")) {
                var path = ConfigurationSection.Path;
                if (!path.Contains(":")) {
                    if (key == ".." || key == "..:") {
                        ConfigurationSection = Configuration.GetSection("");
                        return this;
                    } else
                        throw new ApplicationException($"Cannot navigate to {configKey} from {origPath}");
                } else if (key.StartsWith("..:")) {
                    var parent = path.Substring(0, path.LastIndexOf(":"));
                    ConfigurationSection = Configuration.GetSection(parent);
                    key = key.Substring(3);
                } else {
                    throw new ApplicationException($"Cannot navigate to {configKey} from {origPath}");
                }

            }

            //navigate to the root
            if (key.StartsWith(":")) {
                ConfigurationSection = Configuration.GetSection(key.Substring(1));
            } else if (ConfigurationSection.Path == "") {
                ConfigurationSection = Configuration.GetSection(key);
            } else {
                ConfigurationSection = ConfigurationSection.GetSection(key);
            }

            return this;
        }

    }
}
