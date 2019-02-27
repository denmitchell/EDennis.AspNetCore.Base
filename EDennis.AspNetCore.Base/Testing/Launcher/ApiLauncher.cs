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

namespace EDennis.AspNetCore.Base.Testing {

    public class ApiLauncher<TStartup> : IDisposable
        where TStartup : class {

        private static readonly object _lockobj = new object();


        public ApiLauncher(IConfiguration config, ILogger logger, ProjectPorts projectPorts) {
            _config = config;
            _apis = config.GetApiConfig();
            _args = config.GetCommandLineArguments()
                .Select(x=>$"{x.Key}={x.Value}")
                .ToArray();
            _logger = logger;
            _projectPorts = projectPorts;
        }

        private IConfiguration _config { get; }
        private Dictionary<string,ApiConfig> _apis { get; }
        private string[] _args { get; }
        private string _projectName { get; set; }
        private int _port { get; set; }
        private ILogger _logger { get; }
        private ProjectPorts _projectPorts { get; }

        public async Task StartAsync() {
 

            _projectName = Assembly.GetAssembly(typeof(TStartup)).FullName;
            _projectName = _projectName.Substring(0, _projectName.IndexOf(',')).TrimEnd();

            var apiEntry = _apis.Where(x => x.Value.ProjectName == _projectName).FirstOrDefault();
            var api = apiEntry.Value;

            if (api.BaseAddress != null && api.BaseAddress != "")
                return;

            var configKey = $"Apis:{apiEntry.Key}";


            var dir = api.ProjectDirectory.Replace("{Environment.UserName}", Environment.UserName);

            var projectPortAssignment = _projectPorts.GetProjectPortAssignment(_projectName);
            _port = projectPortAssignment.Port;

            if (projectPortAssignment.AlreadyAssigned) {
                _config[$"{configKey}:BaseAddress"] = $"http://localhost:{_port}";
                return;
            }

            var host = new WebHostBuilder()
            .UseKestrel()
            .UseIISIntegration()
            .UseStartup<TStartup>()
            .UseContentRoot(dir)
            .UseUrls($"http://localhost:{_port}")
            .ConfigureServices(services => {
                services.AddSingleton(_projectPorts);
            })
            .ConfigureAppConfiguration(options => {                
                options.SetBasePath(dir);
                if (_args != null)
                    options.AddCommandLine(_args);
                options.AddJsonFile(dir + "/appsettings.Development.json", true);
            })
                .Build();

            var serverAddresses = host.ServerFeatures.Get<IServerAddressesFeature>();
            var urls = serverAddresses.Addresses.ToArray();

            _logger.LogInformation($"ApiLauncher starting {_projectName} @ {_port}");
            await Task.Run(() => {
                host.WaitForShutdownAsync();
                host.RunAsync();
            });

            _config[$"{configKey}:BaseAddress"] = urls[0];

        }

        private static string AssemblyDirectory(Assembly assembly) {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
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

