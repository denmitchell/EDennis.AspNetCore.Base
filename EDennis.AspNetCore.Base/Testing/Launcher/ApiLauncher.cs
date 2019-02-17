using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.AspNetCore.Base.Testing {

    public class ApiLauncher {

        public ApiLauncher(IConfiguration config, 
            CloneConnections cloneConnections,
            TestInfo testInfo) {
            _config = config;
            _apis = config.GetApiConfig();
            _cloneConnections = cloneConnections;
            _testInfo = testInfo;

            if(testInfo.TestDatabaseType == TestDatabaseType.Clone) {
                DatabaseCloneManager.PopulateCloneConnections(cloneConnections);
            }
        }

        private IConfiguration _config { get; }
        private Dictionary<string,ApiConfig> _apis { get; }
        private CloneConnections _cloneConnections { get; }
        private TestInfo _testInfo { get; }

        public async Task StartAsync<TStartup>(params string[] args) 
            where TStartup : class {
 

            var projectName = Assembly.GetAssembly(typeof(TStartup)).FullName;
            projectName = projectName.Substring(0, projectName.IndexOf(',')).TrimEnd();

            var apiEntry = _apis.Where(x => x.Value.ProjectName == projectName).FirstOrDefault();
            var api = apiEntry.Value;

            if (api.BaseAddress != null && api.BaseAddress != "")
                return;

            var configKey = $"Apis:{apiEntry.Key}";


            var dir = api.ProjectDirectory.Replace("{Environment.UserName}", Environment.UserName);

            Random rand = new Random();
            int startingPort = rand.Next(10000, 63000); 
            var port = PortInspector.GetAvailablePorts(startingPort, 1).FirstOrDefault();

            var host = new WebHostBuilder()
            .UseKestrel()
            .UseIISIntegration()
            .UseStartup<TStartup>()
            .UseContentRoot(dir)
            .UseUrls($"http://localhost:{port}")
            .ConfigureServices(services=> {
                services.AddSingleton(_cloneConnections);
                services.AddSingleton(_testInfo);
            })
            .ConfigureAppConfiguration(options => {                
                options.SetBasePath(dir);
                if (args != null)
                    options.AddCommandLine(args);
                options.AddJsonFile(dir + "/appsettings.Development.json");
            })
                .Build();

            var serverAddresses = host.ServerFeatures.Get<IServerAddressesFeature>();
            var urls = serverAddresses.Addresses.ToArray();

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

    }
}

