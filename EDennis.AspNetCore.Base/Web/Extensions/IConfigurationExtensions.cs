using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace EDennis.AspNetCore.Base.Web {
    public static class IConfigurationExtensions {

        /// <summary>
        /// Gets the database name associated with a named DbContext having
        /// a ConnectionStrings entry.  NOTE: this assumes that the DbContext
        /// class name is the key for the connection string.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static string GetDatabaseName<TContext>(this IConfiguration config)
            where TContext: DbContext{

            var connectionString = config[$"ConnectionStrings:{typeof(TContext).Name}"];
            var builder = new SqlConnectionStringBuilder {
                ConnectionString = connectionString
            };
            return builder.InitialCatalog;
        }


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

        public static Dictionary<string, ApiConfig> GetApiConfig(this IConfiguration config) {

            var env = config["ASPNETCORE_ENVIRONMENT"] ?? "Development";

            var apis = new Dictionary<string, ApiConfig>();
            var apiConfig = config.GetJsonConfiguration($"appsettings.{env}.json")
                .Where(x=>x.Key=="Apis").FirstOrDefault();

            apiConfig.Bind(apis);

            foreach(var api in apis) {
                if (api.Value.ProjectDirectory == null) {
                    api.Value.ProjectDirectory = $"C:/Users/{Environment.UserName}/source/repos/{api.Value.SolutionName}/{api.Value.ProjectName}";
                }
            }

            return apis;
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
