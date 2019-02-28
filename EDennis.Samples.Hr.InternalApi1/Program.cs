using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EDennis.Samples.Hr.InternalApi1.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Hr.InternalApi1 {

    public class Program {
        public static void Main(string[] args) {

            using (var ctx = new HrHistoryContext(
                new DbContextOptionsBuilder<HrHistoryContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Hr;Trusted_Connection=True;")
                .Options
                )) {
                ctx.Database.Migrate();
            }

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
