using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace EDennis.AspNetCore.Base.Web
{
    public static partial class IConfigurationExtensions {


        public static bool ContainsKey(this IConfiguration config, string key) => config.GetSection(key) != null;



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


        /// <summary>
        /// Gets a configuration enumeration for a particular
        /// provider
        /// </summary>
        /// <param name="config">root configuration</param>
        /// <param name="providerType">provider type</param>
        /// <returns>enumerable of configuration sections</returns>
        public static IEnumerable<IConfigurationSection> GetJsonConfiguration(this IConfiguration config, string filePath) {
            var root = config as IConfigurationRoot;
            string path = null;

            //filter the list of providers
            var providers = root.Providers.Where(p => p.GetType() == typeof(JsonConfigurationProvider)).ToList();
            var jsonProviders = providers.Select(p=>p as JsonConfigurationProvider).Where(x => x.Source.Path == filePath).ToList();

            //build the configuration enumeration for the provider.
            //use the Aggregate extension method to build the 
            //configuration cumulatively.
            //(see https://github.com/aspnet/Configuration/blob/master/src/Config/ConfigurationRoot.cs)
            var entries = jsonProviders
                .Aggregate(Enumerable.Empty<string>(),
                    (seed, source) => source.GetChildKeys(seed, path))
                .Distinct()
                .Select(key => GetSection(root, path == null ? key : ConfigurationPath.Combine(path, key)));
            return entries;
        }


        public static void RandomizePorts(this IConfiguration config, string projectName) {
            Apis apis = new Apis();
            config.GetSection("Apis").Bind(apis);
            var apiKey = apis.FirstOrDefault(a => a.Value.ProjectName == projectName).Key;
            var ports = PortManager.GetPorts().Result;
            config[$"Apis:{apiKey}:HttpsPort"] = ports.HttpsPort.ToString();
            config[$"Apis:{apiKey}:HttpPort"] = ports.HttpPort.ToString();
        }


        public static string[] ToCommandLineArgs(this IConfiguration config) {
            List<string> args= new List<string>();
            var flattened = config.Flatten();
            foreach(var entry in flattened) {
                args.Add($"{entry.Key}={entry.Value}");
            }
            return args.ToArray();
        }

        public static Dictionary<string, string> Flatten(this IConfiguration config) {
            var dict = new Dictionary<string, string>();
            config.GetChildren().AsParallel().ToList()
                .ForEach(x => x.Flatten(dict));
            return dict;
        }
        private static void Flatten(this IConfigurationSection section,
            Dictionary<string, string> dict, string parentKey = "") {
            if (section.Value == null)
                section.GetChildren().AsParallel().ToList()
                    .ForEach(x => x.Flatten(dict, $"{parentKey}{section.Key}:"));
            else
                dict.Add($"{parentKey}{section.Key}", section.Value);
        }

    }
}
