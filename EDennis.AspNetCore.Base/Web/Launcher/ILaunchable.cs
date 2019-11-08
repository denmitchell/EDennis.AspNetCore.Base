using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    public interface ILaunchable {

        public static async Task<bool> RunAsync(Type startupClass, ProjectPorts projectPorts, Microsoft.Extensions.Logging.ILogger logger)
                    => await StartupUtils.RunAsync(startupClass, projectPorts, logger, CreateHostBuilder);

        public static IHostBuilder CreateHostBuilder(Type startupClass, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    foreach (var arg in args) {
                        if (arg.StartsWith("ASPNETCORE_URLS=", StringComparison.OrdinalIgnoreCase)) {
                            var urls = arg.Split('=')[1];
                            webBuilder.UseUrls(urls);
                            break;
                        }
                    }
                    //TODO: Add any additional Program-related configuration here.
                    webBuilder.UseStartup(startupClass);
                });
    }
}
