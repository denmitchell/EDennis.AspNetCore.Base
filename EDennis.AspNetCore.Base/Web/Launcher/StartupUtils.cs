using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Web {
    public static class StartupUtils {

        public static async Task<bool> RunAsync(Type startupClass,
            ProjectPorts projectPorts, ILogger logger, Func<Type, string[], IHostBuilder> createHostBuilder) {

            projectPorts.GetOrAddRandomPorts(startupClass, out int[] ports, out string[] portArgs, out bool alreadyAssigned);

            IEnumerable<KeyValuePair<string, object>> dict = new List<KeyValuePair<string, object>> {
                    { KeyValuePair.Create("HttpsPort",(object)ports[0]) },
                    { KeyValuePair.Create("HttpPort",(object)ports[1]) }
                };

            using (logger.BeginScope(dict)) {

                if (alreadyAssigned) {
                    logger.LogInformation("{ProjectName} (a child app) already listening on HTTPS: {HttpsPort}, HTTP: {HttpPort}", startupClass.Assembly.GetName().Name, ports[0], ports[1]);
                    return await Task.FromResult(true);
                }

                var host = createHostBuilder(startupClass, portArgs).Build();
                await Task.Run(() => {
                    host.WaitForShutdownAsync();
                    host.RunAsync();
                    logger.LogInformation("Starting {ProjectName} on HTTPS: {HttpsPort}, HTTP: {HttpPort}", startupClass.Assembly.GetName().Name, ports[0], ports[1]);
                });
                return await IsReady("localhost", ports[0], logger);
            }
        }

        public static async Task<bool> IsReady(string host, int port, ILogger logger, int timeoutSeconds = 5) {

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