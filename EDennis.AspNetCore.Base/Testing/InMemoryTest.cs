using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This abstract class implementings testing using
    /// in-memory databases.  The class handles creation and
    /// disposal of an in-memory testing context.
    /// </summary>
    /// <typeparam name="TContext">The DbContextBase subclass for the test</typeparam>
    public abstract class InMemoryTest<TContext> : IDisposable 
        where TContext : DbContextBase {

        //the entity framework context
        protected TContext Context;

        //the nested dictionary that holds references to 
        //testing instances of DbContextBase
        protected DbContextBaseTestCache dbContextBaseTestCache; 

        //the name used for the test database or transaction
        protected string NamedInstance;

        //the name of the connection string, which must match
        //the dbcontext type name
        protected string ConnectionStringName;

        /// <summary>
        /// Constructs a new InMemoryTest, using configurations
        /// from appsettings.json and appsettings.Development.json
        /// </summary>
        public InMemoryTest() {

            //build configuration from JSON files
            IConfiguration config =
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            //instantiate a new nested dictionary for holding the test dbcontext
            dbContextBaseTestCache = new DbContextBaseTestCache(config);

            //create a new database name and in-memory context
            NamedInstance = Guid.NewGuid().ToString();
            dbContextBaseTestCache.GetOrAddInMemoryContexts(NamedInstance);

            //get the connection string name from the dbcontext class name
            string ConnectionStringName = typeof(TContext).Name;

            //get the testing dbcontext
            var rec = dbContextBaseTestCache.GetDbContexts(NamedInstance)[ConnectionStringName];
            Context = rec as TContext;
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Drops the testing dbcontext instance, if it exists
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if (dbContextBaseTestCache.ContainsKey(NamedInstance)) {
                        dbContextBaseTestCache.DropInMemoryContexts(NamedInstance);
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        #endregion


    }
}
