using EDennis.MigrationsExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This class is used in lieu of providing an OnConfiguring
    /// method and a default constructor in a DbContext class.  The
    /// DbContext subclass looks for a connection string defined in
    /// either appsettings.json or appsettings.Development.json
    /// 
    /// Note: this class also provides support for temporal table
    /// generation via EDennis.MigrationsExtensions
    /// </summary>
    /// <typeparam name="TContext">The DbContextBase type</typeparam>
    /// <seealso cref="SqlContextDesignTimeFactory{TContext}"/>
    public class SqlTemporalContextDesignTimeFactory<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContextBase {

        //holds configuration data
        private IConfiguration _config;

        /// <summary>
        /// Constructs a new SqlContextDesignTimeFactory
        /// and builds the configuration data from an 
        /// appsettings.json file or appsettings.Development.json file
        /// </summary>
        public SqlTemporalContextDesignTimeFactory(){
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
        public SqlTemporalContextDesignTimeFactory(IConfiguration config) {
            _config = config;
        }

        /// <summary>
        /// Builds the DbContextBase object based upon the connection string
        /// from the configuration file.
        /// </summary>
        /// <param name="args">arguments passed to the function</param>
        /// <returns>DbContextBase object</returns>
        public TContext CreateDbContext(string[] args) {

            //create the options builder from the configuration data
            var cxnString = _config[$"ConnectionStrings:{typeof(TContext).Name}"];
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(cxnString);

            //add support for generating temporal tables
            optionsBuilder.ReplaceService<IMigrationsSqlGenerator, TemporalMigrationsSqlGenerator>();

            //use reflection to create the context object
            TContext context = Activator.CreateInstance(typeof(TContext), new object[] { optionsBuilder.Options }) as TContext;

            return context;
        }
    }


}

