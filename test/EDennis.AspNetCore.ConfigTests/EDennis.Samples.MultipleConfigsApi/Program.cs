using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.MultipleConfigsApi {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    var filePrefix = args[0].Split('=')[1];                    
                    webBuilder.ConfigureAppConfiguration((context, configure) => {
                        configure
                            .AddJsonFile($"appsettings\\{filePrefix}.json")
                            .AddJsonFile($"appsettings\\{filePrefix}.Shared.json");
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
