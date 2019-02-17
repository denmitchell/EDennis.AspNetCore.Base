using Microsoft.AspNetCore;
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
        where TContext : DbContext {

        //holds configuration data
        private IConfiguration _config;

        /// <summary>
        /// Overrideable method for building a relevant configuration
        /// </summary>
        /// <returns>Configuration object</returns>
        public virtual IConfiguration BuildConfiguration() {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile($"appsettings.json", true);
            builder.AddJsonFile($"appsettings.{env}.json", true);
            return builder.Build();
        }





        /// <summary>
        /// Builds the DbContextBase object based upon the connection string
        /// from the configuration file.
        /// </summary>
        /// <param name="args">arguments passed to the function</param>
        /// <returns>DbContextBase object</returns>
        public virtual TContext CreateDbContext(string[] args) {

            //create the options builder from the configuration data
            _config = BuildConfiguration();
            var cxnString = _config[$"ConnectionStrings:{typeof(TContext).Name}"];
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlServer(cxnString);

            //use reflection to create the context object
            TContext context = Activator.CreateInstance(typeof(TContext), new object[] { optionsBuilder.Options }) as TContext;

            return context;
        }
    }


}

