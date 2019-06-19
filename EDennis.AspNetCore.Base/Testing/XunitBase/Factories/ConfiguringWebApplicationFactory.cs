using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base.Testing {

    public class ConfiguringWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class{

        public int Port { get; private set; }

        public ConfiguringWebApplicationFactory() {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) {

            var classInfo = new ClassInfo<TStartup>();
            var dir = classInfo.ProjectDirectory;

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            builder
                .UseKestrel()
                .UseStartup<TStartup>()
                .UseContentRoot(dir)
                .ConfigureAppConfiguration(options => {
                    options.SetBasePath(dir);
                    options.AddJsonFile($"appsettings.{env}.json", true);
                    options.AddEnvironmentVariables();
                    options.AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" });
                });

        }


    }
}
