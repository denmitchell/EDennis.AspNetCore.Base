using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.DefaultPoliciesApi.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace EDennis.Samples.DefaultPoliciesApi {

    public class Program {
        public static void Main(string[] args) {
            Task.Run(() => {
                CurrentDirectoryHelpers.SetCurrentDirectory();
            });
            BuildWebHost(args).Run();
        }

        //used by migrations 
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        //used by WebApplicationFactory
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

    }
}
