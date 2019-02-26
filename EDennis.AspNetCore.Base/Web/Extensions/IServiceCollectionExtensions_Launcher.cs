using EDennis.AspNetCore.Base.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    public class ApiLauncherService {
        public IServiceCollection Services { get; set; }
        public ILogger Logger { get; set; }
        public IConfiguration Configuration { get; set; }

        public ApiLauncherService AddLauncher<TStartup>()
            where TStartup : class {
            var launcher = new ApiLauncher<TStartup>(Configuration, Logger);
            Services.AddSingleton(launcher);
            launcher.StartAsync().Wait();
            return this;
        }

        public void AwaitApis() {
            Task.Run(() => {
                while (true) {
                    var apiDict = new Dictionary<string, ApiConfig>();
                    Configuration.GetSection("Apis").Bind(apiDict);
                    var cnt = apiDict.Where(x => x.Value.BaseAddress == "").Count();
                    if (cnt == 0)
                        break;
                    Thread.Sleep(1000);
                }
            });
        }
    }

    public static class IServiceCollectionExtensions_Launcher {


        public static ApiLauncherService AddLauncher<TStartup>(this IServiceCollection services, IConfiguration config, ILogger logger)
            where TStartup : class {
            var launcherService =  new ApiLauncherService {
                 Services = services,
                 Configuration = config,
                 Logger = logger
            };
            return launcherService.AddLauncher<TStartup>();
        }
    }
}

