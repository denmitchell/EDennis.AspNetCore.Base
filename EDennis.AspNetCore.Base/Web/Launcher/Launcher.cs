using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This class moves configuration code from Program.cs
    /// into a separate class that can be packaged into NuGet
    /// and called from a parent application via ApiLauncher middleware.
    /// 
    /// In a typical use case, you would subclass Launcher and then
    /// override the ConfigureDelegate property, if needed.
    /// 
    /// Note that the default implementation of ConfigureDelegate
    /// uses the ManifestEmbeddedFileProvider to load appsettings.json
    /// and appsettings.{env}.json.  This file provider requires 
    /// special setup in the .csproj file.
    /// 
    /// See: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/file-providers?view=aspnetcore-2.1#manifestembeddedfileprovider-1
    /// 
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public abstract class Launcher<TStartup> : ILauncher<TStartup> 
        where TStartup : class {

        public virtual Action<WebHostBuilderContext, IConfigurationBuilder> ConfigureDelegate { get; set; }
            = delegate (WebHostBuilderContext context, IConfigurationBuilder options) {

                var type = MethodBase.GetCurrentMethod().DeclaringType;
                var env = context.HostingEnvironment;

                options.AddJsonFile(new ManifestEmbeddedFileProvider(type.Assembly), $"appsettings.json", true, true);
                options.AddJsonFile(new ManifestEmbeddedFileProvider(type.Assembly), $"appsettings.{env}.json", true, true);
                options.AddEnvironmentVariables();
                options.AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env.EnvironmentName}" });

            };



        public void Run(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public async Task RunAsync(string[] args) {
            var host = CreateHostBuilder(args).Build();
            await host.WaitForShutdownAsync();
            await host.RunAsync();
        }


        public async Task<bool> RunAsync(ApiSettingsFacade api, ILogger logger) {

            IEnumerable<KeyValuePair<string, object>> scope = new List<KeyValuePair<string, object>> {
                    { KeyValuePair.Create("ProjectName",(object)api.ProjectName) },
                    { KeyValuePair.Create("HttpsPort",(object)api.HttpsPort) },
                    { KeyValuePair.Create("HttpPort",(object)api.HttpPort) }
                };

            using (logger.BeginScope(scope)) {

                if (!api.NeedsLaunched) {
                    logger.LogInformation("Skipping launch of {ProjectName}.  Already launched.", typeof(TStartup).Assembly.GetName().Name);
                    return await Task.FromResult(true);
                }
                _ = await UpdatePortsAndVersion(api);

                var host = CreateHostBuilder(api.PortArgs).Build();
                await Task.Run(() => {
                    host.WaitForShutdownAsync();
                    host.RunAsync();
                    if (api.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
                        logger.LogInformation("Starting {ProjectName} on HTTPS: {HttpsPort}, HTTP: {HttpPort}", typeof(TStartup).Assembly.GetName().Name, api.HttpsPort, api.HttpPort);
                    else
                        logger.LogInformation("Starting {ProjectName} on HTTP: {HttpPort}", typeof(TStartup).Assembly.GetName().Name, api.HttpsPort);
                });
                return await IsReady("localhost", api.MainPort, logger);
            }
        }


        private IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    foreach (var arg in args) {
                        if (arg.StartsWith("ASPNETCORE_URLS=", StringComparison.OrdinalIgnoreCase)) {
                            var urls = arg.Split('=')[1];
                            webBuilder.UseUrls(urls);
                            break;
                        }
                    }
                    webBuilder.ConfigureAppConfiguration(ConfigureDelegate);

                    webBuilder.UseStartup<TStartup>();
                });



        private async Task<bool> UpdatePortsAndVersion(ApiSettingsFacade api) {
            if ((api.Scheme ?? "https").Equals("https", StringComparison.OrdinalIgnoreCase)) {
                api.Scheme = "https";
                if (api.HttpsPort == default || api.HttpPort == default) {
                    var ports = await PortInspector.GetRandomAvailablePortsAsync(2);
                    if (api.HttpsPort == default)
                        api.HttpsPort = ports[0];
                    if (api.HttpPort == default)
                        api.HttpPort = ports[1];
                }
            } else {
                api.Scheme = "http";
                if (api.HttpPort == default) {
                    var ports = await PortInspector.GetRandomAvailablePortsAsync(1);
                    api.HttpPort = ports[0];
                }
            }

            api.Version = GetAssemblyVersion(typeof(TStartup).Assembly);

            return true;
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

        private static decimal GetAssemblyVersion(Assembly assembly) {
            return decimal.Parse($"{assembly.GetName().Version.Major}.{assembly.GetName().Version.Minor}");
        }

    }
}
