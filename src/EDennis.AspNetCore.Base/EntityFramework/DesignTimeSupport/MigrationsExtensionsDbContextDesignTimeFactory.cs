using EDennis.MigrationsExtensions;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace EDennis.AspNetCore.Base.EntityFramework {

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
    public class MigrationsExtensionsDbContextDesignTimeFactory<TContext> 
        : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext {

        //holds configuration data
        private IConfiguration _config;

        public virtual ConfigurationType ConfigurationType { get; }

        /// <summary>
        /// Builds the DbContextBase object based upon the connection string
        /// from the configuration file.
        /// </summary>
        /// <param name="args">arguments passed to the function</param>
        /// <returns>DbContextBase object</returns>
        public virtual TContext CreateDbContext(string[] args) {

            

            //create the options builder from the configuration data
            _config = ConfigurationUtils.BuildBuilder(typeof(TContext).Assembly, ConfigurationType, args).Build();


            var configKey = $"DbContexts:{typeof(TContext).Name}:ConnectionString";
            Debug.WriteLine(configKey);

            var cxnString = _config[configKey];

            Debug.WriteLine($"Creating {typeof(TContext).Name} for {cxnString}");

            var builder = new DbContextOptionsBuilder<TContext>();
            builder = builder
                    .UseSqlServer(cxnString)
                    .ReplaceService<IMigrationsSqlGenerator, MigrationsExtensionsSqlGenerator>();

            var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { builder.Options });
            return context;

        }
    }


}

