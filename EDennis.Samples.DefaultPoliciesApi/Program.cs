using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace EDennis.Samples.DefaultPoliciesApi {
    public class Program {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).BuildAndRunWithLauncher();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
