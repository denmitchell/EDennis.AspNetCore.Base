using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This class is used in lieu of providing an OnConfiguring
    /// method and a default constructor in a DbContext class.  The
    /// DbContext subclass looks for a connection string defined in
    /// either appsettings.json or appsettings.Development.json
    /// </summary>
    /// <typeparam name="TContext">The DbContextBase type</typeparam>
    /// <seealso cref="SqlTemporalContextDesignTimeFactory{TContext}"/>
    public class SqlContextDesignTimeFactory<TContext> : IDesignTimeDbContextFactory<TContext> 
        where TContext : DbContextBase {

        //holds configuration data
        private IConfiguration _config;

        /// <summary>
        /// Constructs a new SqlContextDesignTimeFactory
        /// and builds the configuration data from an 
        /// appsettings.json file or appsettings.Development.json file
        /// </summary>
        public SqlContextDesignTimeFactory() {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();
        }

        /// <summary>
        /// Constructs a new SqlContextDesignTimeFactory
        /// and builds the configuration data from the
        /// provided configuration object
        /// </summary>
        /// <param name="config">configuration object</param>
        public SqlContextDesignTimeFactory(IConfiguration config) {
            _config = config;
        }

        /// <summary>
        /// Builds the DbContextBase object based upon the connection string
        /// from the configuration file.
        /// </summary>
        /// <param name="args">arguments passed to the function</param>
        /// <returns>DbContextBase object</returns>
        public virtual TContext CreateDbContext(string[] args) {

            //create the options builder from the configuration data
            var cxnString = _config[$"ConnectionStrings:{nameof(TContext)}"];
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(cxnString);

            //use reflection to create the context object
            TContext context = Activator.CreateInstance(typeof(TContext), new object[] { optionsBuilder.Options }) as TContext;
            context.ConnectionStringName = nameof(TContext);

            return context;
        }
    }


}

