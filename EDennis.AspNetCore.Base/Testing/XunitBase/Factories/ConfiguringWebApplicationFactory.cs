using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;

namespace EDennis.AspNetCore.Base.Testing
{
    //TODO: Determine if this is still needed
    public class ConfiguringWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class{

        public int Port { get; private set; }

        public ConfiguringWebApplicationFactory() {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) {


            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            builder
                .UseKestrel()
                .UseStartup<TStartup>()
                .UseEnvironment(env)
                .ConfigureAppConfiguration(options => {
                    options.AddJsonFile(new ManifestEmbeddedFileProvider(typeof(TStartup).Assembly), $"appsettings.json", true, true);
                    options.AddJsonFile(new ManifestEmbeddedFileProvider(typeof(TStartup).Assembly), $"appsettings.{env}.json", true, true);
                    options.AddEnvironmentVariables();
                    options.AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" });
                });

        }


    }
}
