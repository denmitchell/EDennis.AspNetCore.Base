using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    public static class IConfigurationExtensions {

        /// <summary>
        /// Gets a configuration enumeration for a particular
        /// provider
        /// </summary>
        /// <param name="config">root configuration</param>
        /// <param name="providerType">provider type</param>
        /// <returns>enumerable of configuration sections</returns>
        public static IEnumerable<IConfigurationSection> GetConfiguration(this IConfiguration config, Type providerType) {
            var root = config as IConfigurationRoot;
            string path = null;

            //filter the list of providers
            var providers = root.Providers.Where(p => p.GetType() == providerType);

            //build the configuration enumeration for the provider.
            //use the Aggregate extension method to build the 
            //configuration cumulatively.
            //(see https://github.com/aspnet/Configuration/blob/master/src/Config/ConfigurationRoot.cs)
            var entries = providers
                .Aggregate(Enumerable.Empty<string>(),
                    (seed, source) => source.GetChildKeys(seed, path))
                .Distinct()
                .Select(key => GetSection(root, path == null ? key : ConfigurationPath.Combine(path, key)));
            return entries;
        }

        /// <summary>
        /// Gets a configuartion section
        /// </summary>
        /// <param name="root">root configuration</param>
        /// <param name="key">key for the section</param>
        /// <returns>the configuratio section</returns>
        public static IConfigurationSection GetSection(IConfigurationRoot root, string key) {
            return new ConfigurationSection(root as ConfigurationRoot, key);
        }


        /// <summary>
        /// Gets command-line arguments from configuration
        /// </summary>
        /// <param name="config">the configuration</param>
        /// <returns>dictionary of command-line arguments</returns>
        public static Dictionary<string, string> GetCommandLineArguments(this IConfiguration config) {
            var args = config.GetConfiguration(typeof(CommandLineConfigurationProvider))
                .ToDictionary(x => x.Key, x => x.Value);
            return args;
        }


    }
}
