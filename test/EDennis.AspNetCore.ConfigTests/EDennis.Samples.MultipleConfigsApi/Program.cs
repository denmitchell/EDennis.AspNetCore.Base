using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.MultipleConfigsApi {
    public class Program {

        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static async void RunAsync(string[] args) {
            await CreateHostBuilder(args).Build().RunAsync();
            LauncherUtils.Block(args);
        }

        public static Dictionary<string, Type> Startups = new Dictionary<string, Type>() {
            { "ScopeProperties", typeof(StartupScopeProperties) }
        };

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    var testcase = args[0].Split('=')[1];                    
                    webBuilder.ConfigureAppConfiguration((context, configure) => {
                        configure
                            .AddJsonFile($"appsettings\\{testcase}.json")
                            .AddJsonFile($"appsettings\\{testcase}.Shared.json");
                    });
                    webBuilder.UseStartup(Startups[testcase]);
                });
    }
}
