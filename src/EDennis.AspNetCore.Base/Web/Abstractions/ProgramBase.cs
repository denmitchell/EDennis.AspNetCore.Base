using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Linq;

namespace EDennis.AspNetCore.Base.Web {

    public abstract class ProgramBase<TStartup> : ProgramBase, IProgramBase
        where TStartup : class {
        public override Type Startup {
            get {
                return typeof(TStartup);
            }
        }
    }

    public abstract class ProgramBase : IProgramBase {

        public virtual IConfiguration Configuration {
            get {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
                var config = new ConfigurationBuilder();

                if (UsesEmbeddedConfigurationFiles) {
                    var assembly = Startup.Assembly;
                    var provider = new ManifestEmbeddedFileProvider(assembly);
                    config.AddJsonFile(provider, $"appsettings.json", true, true);
                    config.AddJsonFile(provider, $"appsettings.{env}.json", true, true);

                } else {
                    config.AddJsonFile($"appsettings.json", true, true);
                    config.AddJsonFile($"appsettings.{env}.json", true, true);
                }
                if (UsesSharedConfigurationFile)
                    config.AddJsonFile($"appsettings.Shared.json", true, true);

                config.AddEnvironmentVariables()
                    .AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" });

                return config.Build();
            }
        }
        public virtual bool UsesEmbeddedConfigurationFiles { get; } = true;
        public virtual bool UsesSharedConfigurationFile { get; } = true;
        public virtual string ApisConfigurationSection { get; } = "Apis";
        public abstract Type Startup { get; }

        public async void RunAsync(string[] args) {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public IHostBuilder CreateHostBuilder(string[] args) {

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    var urls = GetUrls();
                    webBuilder
                    .UseConfiguration(Configuration)
                    .UseUrls(urls)
                    .UseStartup(Startup);
                });
            return builder;
        }

        public string[] GetUrls() {
            var apis = new Apis();
            var config = Configuration;
            config.GetSection(ApisConfigurationSection).Bind(apis);
            var api = apis.FirstOrDefault(a => a.Value.ProjectName == GetType().Assembly.GetName().Name).Value;
            return api.Urls;
        }

    }
}
