using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;

namespace EDennis.AspNetCore.Base.Testing
{
    /// <summary>
    /// Note: Add appsettings.json, appsettings.{env}.json, and 
    /// (optionally) appsettings.{Shared}.json to the test project.
    /// If using Visual Studio, add these files as linked files.
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
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
                    options.AddJsonFile($"appsettings.json", true, true);
                    options.AddJsonFile($"appsettings.{env}.json", true, true);
                    options.AddJsonFile($"appsettings.Shared.json", true, true);
                    options.AddEnvironmentVariables();
                    options.AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" });
                });

        }


    }
}
