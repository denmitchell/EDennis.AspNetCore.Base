using ConfigCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Linq;
using System.Reflection;

namespace EDennis.AspNetCore.Base {
    public static class ConfigurationUtils {


        public static IConfigurationBuilder BuildBuilder(
            Assembly projectAssembly,
            ConfigurationType configurationType = ConfigurationType.PhysicalFiles,
            string[] args = null) {

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var apiUrl = "ConfigurationApiUrl";
            var apiKey = "ConfigurationApiKey";

            var configBuilder = new ConfigurationBuilder();
            var projectName = projectAssembly.GetName().Name.Replace(".Lib", "");

            switch (configurationType) {
                case ConfigurationType.ConfigurationApi:
                    configBuilder.AddApiSource(apiUrl, "ApiKey", apiKey, projectName, false);
                    break;
                case ConfigurationType.ManifestedEmbeddedFiles:
                    ManifestEmbeddedFileProvider provider = new ManifestEmbeddedFileProvider(projectAssembly);
                    configBuilder.AddJsonFile(provider, $"ProjectRoot\\{projectName}\\appsettings.json", true, true);
                    configBuilder.AddJsonFile(provider, $"ProjectRoot\\{projectName}\\appsettings.{env}.json", true, true);
                    configBuilder.AddJsonFile(provider, $"appsettings.json", true, true);
                    configBuilder.AddJsonFile(provider, $"appsettings.{env}.json", true, true);
                    configBuilder.AddJsonFile($"appsettings.Launcher.json", true, true);
                    break;
                default:
                    configBuilder.AddJsonFile($"ProjectRoot\\{projectName}\\appsettings.json", true, true);
                    configBuilder.AddJsonFile($"ProjectRoot\\{projectName}\\appsettings.{env}.json", true, true);
                    configBuilder.AddJsonFile($"appsettings.json", true, true);
                    configBuilder.AddJsonFile("appsettings.{env}.json", true, true);
                    configBuilder.AddJsonFile($"appsettings.Launcher.json", true, true);
                    break;
            }

            configBuilder.AddEnvironmentVariables();
            if (args != null)
                configBuilder.AddCommandLine(args);

            configBuilder.AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" });

            return configBuilder;

        }


    }
}
