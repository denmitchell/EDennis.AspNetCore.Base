using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;

namespace EDennis.Samples.MockClientMiddlewareApi {
    public class Program {
        public static int Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .MinimumLevel.Override("System", LogEventLevel.Verbose)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Verbose)
                .Enrich.FromLogContext()
                .WriteTo.File(@"mockclientapi_log.txt")
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            try {
                Log.Information("Starting host...");
                CreateHostBuilder(args).Build().Run();
                return 0;
            } catch (Exception ex) {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new Lib.Program().CreateHostBuilder(args);

    }
}