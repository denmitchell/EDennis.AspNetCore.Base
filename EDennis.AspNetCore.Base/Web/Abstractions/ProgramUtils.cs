using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base {
    public static class ProgramUtils {

        public const string APIS_CONFIGURATION_SECTION = "Apis";


        public static IHostBuilder CreateHostBuilder<TProgram, TStartup>(string[] args)
            where TProgram : class
            where TStartup : class
            => CreateHostBuilder<TProgram, TStartup>(args, GetConfiguration<TProgram>(),APIS_CONFIGURATION_SECTION);


        public static IHostBuilder CreateHostBuilder<TProgram, TStartup>(string[] args, IConfiguration configuration, string apisConfigurationSection = "Apis")
            where TProgram : class
            where TStartup : class {

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    var urls = GetUrls<TProgram>(configuration);
                    webBuilder
                        .UseConfiguration(configuration)
                        .UseUrls(urls)
                        .UseStartup<TStartup>();
                });
            return builder;
        }

        public static string[] GetUrls<TProgram>(IConfiguration config)
            where TProgram : class {
            var apis = new Apis();
            config.GetSection(APIS_CONFIGURATION_SECTION).Bind(apis);
            var api = apis.FirstOrDefault(a => a.Value.ProjectName == typeof(TProgram).Assembly.GetName().Name).Value;
            return api.Urls;
        }

        public static IConfiguration GetConfiguration<TProgram>()
            where TProgram : class {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var assembly = typeof(TProgram).Assembly;
            var provider = new ManifestEmbeddedFileProvider(assembly);
            var config = new ConfigurationBuilder()
                .AddJsonFile(provider, $"appsettings.json", true, true)
                .AddJsonFile(provider, $"appsettings.{env}.json", true, true)
                .AddJsonFile($"appsettings.Shared.json", true)
                .Build();
            return config;
        }


    }
}
