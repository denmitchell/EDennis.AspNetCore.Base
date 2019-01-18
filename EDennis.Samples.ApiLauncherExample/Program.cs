using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using EDennis.AspNetCore.Base.Web;

namespace EDennis.Samples.ApiLauncherExample {
    public class Program {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).BuildAndRunWithLauncher();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .UseIIS();
    }
}
