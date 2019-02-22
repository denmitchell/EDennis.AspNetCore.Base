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


        public static TContext CreateInMemoryDatabase(string baseDatabaseName, string instanceName) {
            var options = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase($"{baseDatabaseName}-{instanceName}")
                .Options;

            //using reflection, instantiate the DbContext subclass
            var dbContext = Activator.CreateInstance(typeof(TContext),
                new object[] { options }) as TContext;

            dbContext.Database.EnsureCreated();
            return dbContext;

        }


        public static void DropInMemoryDatabase(TContext context) {
            context.Database.EnsureDeleted();
        }



        public static TContext GetReadonlyDatabase(string databaseName) {

            var cxnString = MssqlLocalDbConnectionString(databaseName);

            var options = new DbContextOptionsBuilder<TContext>()
                .UseSqlServer(cxnString)
                .Options;

            //using reflection, instantiate the DbContextBase subclass
            var context = Activator.CreateInstance(typeof(TContext),
                new object[] { options }) as TContext;

            return context;
        }

        public static TContext GetReadonlyDatabase(IConfiguration config) {

            var databaseName = config.GetDatabaseName<TContext>();
            var cxnString = MssqlLocalDbConnectionString(databaseName);

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


        public static string MssqlLocalDbConnectionString(string databaseName) {
            return $@"Server=(localdb)\mssqllocaldb;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true";
        }
    }

}

