using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace EDennis.Samples.DefaultPoliciesApi {

    public class Program {
        public static void Main(string[] args) {
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
