using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using EDennis.AspNetCore.Base.Web;
using System.Reflection.Emit;
using System.Reflection;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Testing {
    public class TestDbContextManager<TContext>
        where TContext : DbContext {


        public static void CreateInMemoryDatabase(
                string baseDatabaseName, string instanceName,
                out DbContextOptions<TContext> options, out TContext context ) {

            options = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase($"{baseDatabaseName}-{instanceName}")
                .Options;

            //using reflection, instantiate the DbContext subclass
            context = Activator.CreateInstance(typeof(TContext),
                new object[] { options }) as TContext;

            context.Database.EnsureCreated();
        }

        public static void CreateInMemoryDatabase(
            DbContextOptions<TContext> options, out TContext context) {

            //using reflection, instantiate the DbContext subclass
            context = Activator.CreateInstance(typeof(TContext),
                new object[] { options }) as TContext;

            context.Database.EnsureCreated();
        }


        public static void DropInMemoryDatabase(DbContextOptions<TContext> options) {
            
            //using reflection, instantiate the DbContext subclass
            var context = Activator.CreateInstance(typeof(TContext),
                new object[] { options }) as TContext;

            context.Database.EnsureDeleted();
        }


        public static void DropInMemoryDatabase(TContext context) {
            context.Database.EnsureDeleted();
        }

        public static TContext GetReadonlyDatabase(IConfiguration config) {

            var contextName = typeof(TContext).Name;

            var cxnString = config[$"ConnectionStrings:{contextName}"];

            var options = new DbContextOptionsBuilder<TContext>()
                .UseSqlServer(cxnString)
                .Options;

            //using reflection, instantiate the DbContextBase subclass
            var context = Activator.CreateInstance(typeof(TContext),
                new object[] { options }) as TContext;

            return context;
        }



        public static string BaseDatabaseName(IConfiguration config) {
            return config.GetDatabaseName<TContext>();
        }



        public static string DatabaseName(string connectionString) {
            var builder = new SqlConnectionStringBuilder {
                ConnectionString = connectionString
            };
            return builder.InitialCatalog;
        }


    }

}

