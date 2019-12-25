using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace EDennis.Samples.DbContextConfigsApi {
    public class Program {
        public static void Main(string[] args) {    
            //if(args != null && args.Length > 0 && args[0].StartsWith("UpdateDatabase"))
            //    UpdateDatabase(args[0].Substring(args[0].LastIndexOf('=')+1));
            //else
                CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new Lib.Program().CreateHostBuilder(args);

        // public static void UpdateDatabase(string dbContext) {
            // var prog = new Lib.Program();

            // if(dbContext == "DbContext1")
                // prog.UpdateDatabase<DbContext1>();
            // else if (dbContext == "DbContext2")
                // prog.UpdateDatabase<DbContext2>();
            // else if (dbContext == "DbContext3")
                // prog.UpdateDatabase<DbContext3>();
        // }
    }
}