using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    public abstract class ProgramBase<TStartup> : ProgramBase 
        where TStartup : class
        {
        public override Type Startup {
            get {
                return Startup;
            }
        }
    }

    public abstract class ProgramBase {

        public virtual IConfiguration Configuration { 
            get {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
                var config = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{env}.json", true, true)
                    .AddJsonFile($"appsettings.Shared.json", true, true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" })
                    .Build();
                return config;
            }
        }
        public virtual string ApisConfigurationSection { get; } = "Apis";
        public abstract Type Startup { get; }

        public async Task RunAsync(string[] args) {
            await CreateHostBuilder(args).Build().RunAsync();
            return;
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
