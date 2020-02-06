using ConfigurationApi.Lib.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ConfigurationApiServer.Lib {
    public class Program {

        public static void Main(string[] args) {
            var configurationManager = new ConfigurationManager();
            Task.Run(async()=> { await configurationManager.UploadNew(); });
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
