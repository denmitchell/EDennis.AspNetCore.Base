using EDennis.MigrationsExtensions;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;

namespace ConfigurationApi.Lib.Models {

    /// <summary>
    /// This class is used in lieu of providing an OnConfiguring
    /// method and a default constructor in a DbContext class.  The
    /// DbContext subclass looks for a connection string defined in
    /// either appsettings.json or appsettings.Development.json.
    /// NOTE: only works with SQL Server
    /// NOTE: requires that DbContext injects DbContextOptionsProvider,
    ///       rather than DbContextOptions
    /// </summary>
    /// <typeparam name="TContext">The DbContextBase type</typeparam>
    public class ConfigurationDbContextDesignTimeFactory 
        : IDesignTimeDbContextFactory<ConfigurationDbContext>{

        //holds configuration data
        private IConfiguration _config;

        /// <summary>
        /// Overrideable method for building a relevant configuration
        /// </summary>
        /// <returns>Configuration object</returns>
        public virtual IConfiguration BuildConfiguration() {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(ConfigurationApiConfiguration.GetConfiguration(env));
            return builder.Build();
        }





        /// <summary>
        /// Builds the DbContextBase object based upon the connection string
        /// from the configuration file.
        /// </summary>
        /// <param name="args">arguments passed to the function</param>
        /// <returns>DbContextBase object</returns>
        public virtual ConfigurationDbContext CreateDbContext(string[] args) {

            //create the options builder from the configuration data
            _config = BuildConfiguration();

            var cxnString = _config["DbContexts:ConfigurationDbContext:ConnectionString"];

            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            builder = builder
                    .UseSqlServer(cxnString)
                    .ReplaceService<IMigrationsSqlGenerator, MigrationsExtensionsSqlGenerator>();

            var context = new ConfigurationDbContext(builder.Options);
            return context;

        }
    }


}

