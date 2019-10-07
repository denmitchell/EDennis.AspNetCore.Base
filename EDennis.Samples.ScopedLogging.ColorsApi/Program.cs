using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace EDennis.Samples.ScopedLogging.ColorsApi {
    public class Program {
        public static void Main(string[] args) {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            //This is the default logger.
            //   Name = "Logger", Index = 0, LogLevel = Information
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration, "Logging:Loggers:Default")
                        .CreateLogger();

            Log.Logger.Information("Hello from Logger!");


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder
                    .UseSerilog()
                    .UseStartup<Startup>();
                });
    }
}
