using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This abstract class implementings testing using
    /// a SQL Server database.  The class handles creation and
    /// disposal of the testing context.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class QueryTest<TContext>  
        where TContext : DbContextBase {

        //the entity framework context
        protected TContext Context;


        /// <summary>
        /// Constructs a new query test using the provided
        /// entity framework context options
        /// </summary>
        /// <param name="options">options for the entity framework context</param>
        public QueryTest(DbContextOptions options) {
            Context = Activator.CreateInstance(typeof(TContext), new object[] { options }) as TContext;
        }

        /// <summary>
        /// Constructs a new QueryTest against a SQL Server database, 
        /// using configurations from appsettings.json and 
        /// appsettings.Development.json
        /// </summary>
        public QueryTest() {

            //build configuration from JSON files
            IConfiguration config =
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            //get the connection strings from config
            var _cxns = config.GetSection("ConnectionStrings").GetChildren();


            //get the connection string name from the dbcontext class name
            string ConnectionStringName = typeof(TContext).Name;

            //get the target connection string
            string connectionString = _cxns.Where(x => x.Key == ConnectionStringName).FirstOrDefault().Value;

            //create options for SQL Server
            var options = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString);

            Context = Activator.CreateInstance(typeof(TContext), new object[] { options }) as TContext;
        }


    }
}
