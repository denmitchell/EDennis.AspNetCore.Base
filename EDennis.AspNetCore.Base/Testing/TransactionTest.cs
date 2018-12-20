using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This abstract class implementings testing using
    /// testing transactions.  The class handles creation and
    /// disposal of the testing context.  Note: due to the 
    /// isolation level of the testing transactions, database locking
    /// can occur, leading to failed tests.
    /// </summary>
    /// <typeparam name="TContext">The DbContextBase subclass for the test</typeparam>
    public abstract class TransactionTest<TContext> : IDisposable 
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
        /// Constructs a new TransactionTest, using configurations
        /// from appsettings.json and appsettings.Development.json
        /// </summary>
        public TransactionTest() {

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
            dbContextBaseTestCache.GetOrAddTestingTransactionContexts(NamedInstance);

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
                        dbContextBaseTestCache.DropTestingTransactionContexts(NamedInstance);
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
