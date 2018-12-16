using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using EDennis.AspNetCore.Base.Web;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// <para>
    ///   Specifically designed to be instantiated as a singleton,
    ///   this class provides holds test contexts that extend 
    ///   DbContextBase.  The test contexts support in-memory 
    ///   databases.
    /// </para>
    /// <para>
    ///   Each DbContextBase instance is keyed in two ways:
    ///   (1) NamedInstance (string) -- a GUID used to keep track
    ///       of a specific testing instance of a database
    ///   (2) ConnectionStringName (string) -- also the same as the
    ///       class name, this is used to differentiate two or more 
    ///       DbContextBase subclasses. 
    /// </para>
    /// <para>
    ///   Testing Note: Currently, all tests must be run sequentially;
    ///   otherwise, value generators are not reset correctly.  That
    ///   said, this class is designed to allow parallel tests in the
    ///   future.  Assuming each test instance uses a unique 
    ///   NamedInstance, this class will be thread safe.
    /// </para>
    /// </summary>
    public class DbContextBaseTestCache : Dictionary<string, Dictionary<string, DbContextBase>>, IDbContextBaseTestCache {

        //holds connection strings from configuration data
        private readonly IEnumerable<IConfigurationSection> _cxns;

        //holds all DbContextBase subclass types
        private readonly IEnumerable<Type> _dbContextTypes;

        //true, if sequences need to be reset when an in-memory
        //database is dropped or testing-transaction is rolled back
        private readonly bool _resetSequences = true;

        //true, if identities need to be reset when an in-memory
        //database is dropped or testing-transaction is rolled back
        private readonly bool _resetIdentities = true;


        /// <summary>
        /// Constructs a new DbContextBaseTestCache, based upon
        /// the provided configuration data.
        /// ASSUMPTION: There are the following sections in the
        /// configuration file or in environment variables:
        /// ConnectionStrings:{DbContextBaseClassName}
        /// Testing:ResetSequences
        /// Testing:ResetIdentities
        /// </summary>
        /// <param name="config">configuration data</param>
        public DbContextBaseTestCache(IConfiguration config) {

            //get all connection strings
            _cxns = config.GetSection("ConnectionStrings").GetChildren();

            //get a collection of DbContextBase types
            _dbContextTypes = GetDbContextTypes();

            //get setting for resetting sequences
            var resetSequences = config["Testing:ResetSequences"];
            if (resetSequences != null)
                _resetSequences = bool.Parse(resetSequences.ToLower());


            //get setting for resetting identities
            var resetIdentities = config["Testing:ResetIdentities"];
            if (resetIdentities != null)
                _resetIdentities = bool.Parse(resetIdentities.ToLower());

        }


        /// <summary>
        /// Gets a dictionary of DbContextBase subclasses, 
        /// keyed by the ConnectionStringName (which also should
        /// be the name of the DbContextBase subclass itself).
        /// If the dictionary doesn't exist, create it first.
        /// All entries in this dictionary will have
        /// connections to different in-memory databases.
        /// </summary>
        /// <param name="namedInstance">The GUID key for the 
        /// testing instance</param>
        /// <returns>a dictionary of DbContextBase classes
        /// keyed by the ConnectionStringName</returns>        
        public Dictionary<string, DbContextBase> GetOrAddInMemoryContexts(string namedInstance) {

            //get the current NamedInstance of DbContextBase classes
            if (ContainsKey(namedInstance))
                return this[namedInstance];

            //otherwise ...

            //create a new dictionary to hold all classes for this named instance
            var dict = new Dictionary<string, DbContextBase>();

            //iterate over all subclasses of DbContextBase
            foreach (var contextType in _dbContextTypes) {

                //get the relevant connection string
                var cxn = _cxns.Where(c => c.Key == contextType.Name)
                    .FirstOrDefault();

                //construct a unique database name based upon
                //named instance and connection string name
                var dbName = $"{namedInstance}:{cxn.Key}";

                //build the options for the database
                var options = new DbContextOptionsBuilder()
                    .UseInMemoryDatabase(dbName)
                    .Options;

                //using reflection, instantiate the DbContextBase subclass
                var context = Activator.CreateInstance(contextType, 
                    new object[] { options }) as DbContextBase;

                //set properties for the current DbContextBase 
                context.NamedInstance = namedInstance;
                context.ConnectionStringName = cxn.Key;

                //generate the database and data
                context.Database.EnsureCreated();

                //add the current DbContextBase to the dictionary
                dict.Add(cxn.Key, context);
            }

            //add the dictionary to the cache
            Add(namedInstance, dict);

            //return the dictionary
            return dict;
        }


        /// <summary>
        /// Drops the in-memory database from the cache
        /// </summary>
        /// <param name="namedInstance">The GUID key for the 
        /// testing instance</param>
        public void DropInMemoryContexts(string namedInstance) {

            //retrieve a reference to the cache entry
            var dict = this[namedInstance];

            //iterate over all DbContextBase subclasses in the 
            //retrieved cache entry
            foreach (string key in dict.Keys) {
                //get a reference to the current DbContextBase subclass
                var context = dict[key];
                context.Database.EnsureDeleted();
            }
            //remove the named instance from the cache
            Remove(namedInstance);
        }

        /// <summary>
        /// Gets a dictionary of DbContextBase subclasses, 
        /// keyed by the ConnectionStringName (which also should
        /// be the name of the DbContextBase subclass itself).
        /// If the dictionary doesn't exist, create it first.
        /// All entries in this dictionary will have different
        /// connections governed by testing transactions.
        /// </summary>
        /// <param name="namedInstance">The GUID key for the 
        /// testing instance</param>
        /// <returns>a dictionary of DbContextBase classes
        /// keyed by the ConnectionStringName</returns>        
        public Dictionary<string, DbContextBase> GetOrAddTestingTransactionContexts(string namedInstance) {

            //get the current NamedInstance of DbContextBase classes
            if (ContainsKey(namedInstance))
                return this[namedInstance];

            //otherwise ...

            //create a new dictionary to hold all classes for this named instance
            var dict = new Dictionary<string, DbContextBase>();

            //iterate over all subclasses of DbContextBase
            foreach (var contextType in _dbContextTypes) {

                //get the relevant connection string
                var cxn = _cxns.Where(c => c.Key == contextType.Name)
                    .FirstOrDefault();

                //create a new connection to SQL Server
                var connection = new SqlConnection(cxn.Value);

                //open the connection and start a transaction
                connection.Open();

                //proactively reset sequences and identities that
                //might have not been reset during the last session
                if (_resetSequences)
                    connection.ResetSequences();
                if (_resetIdentities)
                    connection.ResetIdentities();

                var transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                //create the options for the DbContextBase subclass
                var options = new DbContextOptionsBuilder()
                    .UseSqlServer(connection)
                    .Options;

                //using reflection, instantiate the DbContextBase subclass
                var context = Activator.CreateInstance(contextType,
                    new object[] { options }) as DbContextBase;

                //attach the transaction to the current connection 
                context.Database.UseTransaction(transaction);
                context.Database.AutoTransactionsEnabled = false;

                context.HasSequences = _resetSequences;
                context.HasIdentities = _resetIdentities;

                //set properties for the current DbContextBase 
                context.NamedInstance = namedInstance;
                context.ConnectionStringName = cxn.Key;

                //add the current DbContextBase to the dictionary
                dict.Add(cxn.Key, context);
            }

            //add the dictionary to the cache
            Add(namedInstance, dict);

            //return the dictionary
            return dict;
        }


        /// <summary>
        /// Rolls back a testing transaction, resets identities
        /// or sequences (as needed), and removes the named instance
        /// of DbContextBase subclasses from the cache.
        /// </summary>
        /// <param name="namedInstance">The GUID key for the 
        /// testing instance</param>
        public void DropTestingTransactionContexts(string namedInstance) {

            //retrieve a reference to the cache entry
            var dict = this[namedInstance];

            //iterate over all DbContextBase subclasses in the 
            //retrieved cache entry
            foreach (string key in dict.Keys) {

                //rollback the transaction
                dict[key].Database.RollbackTransaction();

                //get the connection
                var cxn = dict[key].Database.GetDbConnection() as SqlConnection;

                //reset identities and/or sequences, as needed
                if (_resetIdentities)
                    cxn.ResetIdentities();
                if (_resetSequences)
                    cxn.ResetSequences();
            }
            //remove the named instance from the cache
            Remove(namedInstance);
        }

        /// <summary>
        /// Retrieves a dictionary of DbContextBase subclasses
        /// by named instance
        /// </summary>
        /// <param name="namedInstance">The GUID key for the 
        /// testing instance</param>
        /// <returns>a dictionary of DbContextBase classes
        /// keyed by the ConnectionStringName</returns>        
        public Dictionary<string,DbContextBase> GetDbContexts(string namedInstance) {
            return this[namedInstance];
        }

        /// <summary>
        /// Using reflection, retrieves a collection of all
        /// DbContextBase subclasses
        /// </summary>
        /// <returns>DbContextBase subclasses</returns>
        private IEnumerable<Type> GetDbContextTypes() {
            var serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(DbContextBase)));
            serviceTypes = serviceTypes.Where(s => _cxns.Select(c => c.Key).Contains(s.Name));
            return serviceTypes;
        }

    }
}
