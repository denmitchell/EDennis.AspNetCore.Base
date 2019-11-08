using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    public class Launcher<TStartup>
        where TStartup : class {

        public virtual async Task<bool> RunAsync(LauncherSettingsDictionary dict, ILogger logger)
                    => await RunAsync(dict, logger, CreateHostBuilder);

        public virtual IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    foreach (var arg in args) {
                        if (arg.StartsWith("ASPNETCORE_URLS=", StringComparison.OrdinalIgnoreCase)) {
                            var urls = arg.Split('=')[1];
                            webBuilder.UseUrls(urls);
                            break;
                        }
                    }
                    webBuilder.ConfigureAppConfiguration((context, options) => {
                        var env = context.HostingEnvironment;
                        options.AddJsonFile(new ManifestEmbeddedFileProvider(GetType().Assembly), $"appsettings.{env}.json", true, true);
                        options.AddEnvironmentVariables();
                        options.AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env.EnvironmentName}" });
                    });

                    webBuilder.UseStartup<TStartup>();
                });


        public async Task<bool> RunAsync(
            LauncherSettingsDictionary dict, ILogger logger, Func<string[], IHostBuilder> createHostBuilder) {

            dict.GetOrAddLauncherSettings(dict);

            IEnumerable<KeyValuePair<string, object>> scope = new List<KeyValuePair<string, object>> {
                    { KeyValuePair.Create("HttpsPort",(object)ports[0]) },
                    { KeyValuePair.Create("HttpPort",(object)ports[1]) }
                };

            using (logger.BeginScope(scope)) {

                if (alreadyAssigned) {
                    logger.LogInformation("{ProjectName} (a child app) already listening on HTTPS: {HttpsPort}, HTTP: {HttpPort}", typeof(TStartup).Assembly.GetName().Name, ports[0], ports[1]);
                    return await Task.FromResult(true);
                }

                var host = createHostBuilder(portArgs).Build();
                await Task.Run(() => {
                    host.WaitForShutdownAsync();
                    host.RunAsync();
                    logger.LogInformation("Starting {ProjectName} on HTTPS: {HttpsPort}, HTTP: {HttpPort}", typeof(TStartup).Assembly.GetName().Name, ports[0], ports[1]);
                });
                return await IsReady("localhost", ports[0], logger);
            }
        }

        private async Task<bool> IsReady(string host, int port, ILogger logger, int timeoutSeconds = 5) {

            var sw = new Stopwatch();

            sw.Start();
            while (sw.ElapsedMilliseconds < (timeoutSeconds * 1000)) {
                try {
                    using var tcp = new TcpClient(host, port);
                    var connected = tcp.Connected;
                    logger.LogInformation("Successful ping of http:\\\\{Host}:{Port}", host, port);
                    return await Task.FromResult(true);
                } catch (Exception ex) {
                    if (!ex.Message.Contains("No connection could be made because the target machine actively refused it")) {
                        var aex = new ApplicationException($"Could not ping http:\\\\{host}:{port}", ex);
                        logger.LogError(aex, "Could not ping http:\\\\{Host}:{Port}", host, port);
                        throw aex;
                    } else
                        Thread.Sleep(1000);
                }
            }
            return await Task.FromResult(false);

        }


    }
}
