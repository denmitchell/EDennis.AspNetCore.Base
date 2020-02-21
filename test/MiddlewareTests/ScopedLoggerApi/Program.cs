using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace ScopedLoggerApi {
    public class Program {
        public static void Main(string[] args) {
             CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {

            Log.Logger = new LoggerConfiguration()
                       .Enrich.FromLogContext()
                       .MinimumLevel.Information()
                       .WriteTo.File("log.txt")
                       .CreateLogger();

            try {
                Log.Information("Starting up");
            } catch (Exception ex) {
                Log.Fatal(ex, "Application start-up failed");
            } finally {
                Log.CloseAndFlush();
            }


            return new Lib.Program().CreateHostBuilder(args)
            .ConfigureLogging(x => x.ClearProviders())
            .UseSerilog();
        }
    }
}