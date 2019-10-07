using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Diagnostics;
using System.Net.Sockets;

namespace EDennis.AspNetCore.Base.Testing {

    public class ApiLauncher<TStartup> : IDisposable
        where TStartup : class {


        public ApiLauncher(IConfiguration config, ILogger logger, ProjectPorts projectPorts) {
            _config = config;
            _apis = config.GetApiConfig();
            _args = config.GetCommandLineArguments()
                .Select(x => $"{x.Key}={x.Value}")
                .ToArray();
            _logger = logger;
            _projectPorts = projectPorts;
        }

        private readonly IConfiguration _config;
        private readonly Dictionary<string, ApiConfig> _apis;
        private readonly string[] _args;
        private string _projectName;
        private int _port;
        private readonly ILogger _logger;
        private readonly ProjectPorts _projectPorts;

        public async Task StartAsync() {


            _projectName = Assembly.GetAssembly(typeof(TStartup)).FullName;
            _projectName = _projectName.Substring(0, _projectName.IndexOf(',')).TrimEnd();

            var apiEntry = _apis.Where(x => x.Value.ProjectName == _projectName).FirstOrDefault();

            if (apiEntry.Equals(default(KeyValuePair<string, ApiConfig>)))
                throw new ApplicationException($"Cannot find configuration entry (e.g., in appsettings.Development.json) for an Api whose project name is '{_projectName}'");


            var api = apiEntry.Value;

            if (api.ExternallyLaunched || (api.BaseAddress != null && api.BaseAddress != "" 
                && !api.BaseAddress.Contains("localhost")))
                return;

            var configKey = $"Apis:{apiEntry.Key}";


            var dir = api.ProjectDirectory.Replace("{Environment.UserName}", Environment.UserName);

            if (!api.BaseAddress.Contains("localhost")) {
                //assign a new port to the project or get the current port assignment
                var projectPortAssignment = _projectPorts.GetProjectPortAssignment(_projectName);
                _port = projectPortAssignment.Port;

                //short-circuit if the project has been assigned a port, already
                if (projectPortAssignment.AlreadyAssigned) {
                    _config[$"{configKey}:BaseAddress"] = $"http://localhost:{_port}";
                    _config[$"{configKey}:Pingable"] = "true";
                    return;
                }
            } else {
                _port = int.Parse(api.BaseAddress.Replace("https", "http").Replace("http://localhost:", ""));
            }

            var env = _config["ASPNETCORE_ENVIRONMENT"];


            var host = new WebHostBuilder()
            .UseKestrel()
            .UseStartup<TStartup>()
            .UseContentRoot(dir)
            .UseUrls($"http://localhost:{_port}")
            .ConfigureLogging(options => {
                options.AddConsole().AddDebug();
            })
            .ConfigureServices(services => {
                services.AddSingleton(_projectPorts);
            })
            .ConfigureAppConfiguration(options => {
                options.SetBasePath(dir);
                options.AddJsonFile($"appsettings.{env}.json", true);
                if (_args != null)
                    options.AddCommandLine(_args);
            })
                .Build();

            var serverAddresses = host.ServerFeatures.Get<IServerAddressesFeature>();
            var urls = serverAddresses.Addresses.ToArray();

            _logger.LogInformation($"ApiLauncher starting {_projectName} @ {_port}");
            await Task.Run(() => {
                host.WaitForShutdownAsync();
                host.RunAsync();
            });

            _config[$"{configKey}:BaseAddress"] = $"http://localhost:{_port}";

            await Task.Run(() => {
                Ping(configKey,"localhost", _port);
            });


        }

        private static string AssemblyDirectory(Assembly assembly) {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }


        private void Ping(string configKey, string host, int port, int timeoutSeconds = 5) {

            _config[$"{configKey}:Pingable"] = "false";

            var sw = new Stopwatch();

            sw.Start();
            while (sw.ElapsedMilliseconds < (timeoutSeconds * 1000)) {
                try {
                    using (var tcp = new TcpClient(host, port)) {
                        var connected = tcp.Connected;
                        _config[$"{configKey}:Pingable"] = "true";
                        break;
                    }
                } catch (Exception ex) {
                    if (!ex.Message.Contains("No connection could be made because the target machine actively refused it"))
                        throw new ApplicationException($"Could not ping http:\\localhost:{port}",ex);
                    else
                        Thread.Sleep(1000);
                }

            }

        }





        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _logger.LogInformation($"ApiLauncher stopping {_projectName} @ {_port}");
                    // TODO: dispose managed state (managed objects).
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            Dispose(true);
        }
        #endregion




    }
}

