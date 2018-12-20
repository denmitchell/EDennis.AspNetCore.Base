using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.EntityFramework {
    /// <summary>
    /// <para>
    ///   Specifically designed to be instantiated as a singleton,
    ///   this interface specifies handling of test contexts that extend 
    ///   DbContextBase.  The test contexts support both
    ///   (a) in-memory databases AND
    ///   (b) "testing-transaction" connections to real databases
    ///   The latter can be used to rollback to a pre-test state.
    /// </para>
    /// <para>
    ///   Each DbContextBase instance is keyed in two ways:
    ///   (1) NamedInstance (string) -- a GUID used to keep track
    ///       of a specific testing instance of a database or connection
    ///   (2) ConnectionStringName (string) -- also the same as the
    ///       class name, this is used to differentiate two or more 
    ///       DbContextBase subclasses. 
    /// </para>
    /// </summary>
    public interface IDbContextBaseTestCache {

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
        Dictionary<string, DbContextBase> GetOrAddInMemoryContexts(string namedInstance);


        /// <summary>
        /// Drops the in-memory database from the cache
        /// </summary>
        /// <param name="namedInstance">The GUID key for the 
        void DropInMemoryContexts(string namedInstance);

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
        Dictionary<string, DbContextBase> GetOrAddTestingTransactionContexts(string namedInstance);


        /// <summary>
        /// Rolls back a testing transaction, resets identities
        /// or sequences (as needed), and removes the named instance
        /// of DbContextBase subclasses from the cache.
        /// </summary>
        /// <param name="namedInstance">The GUID key for the 
        /// testing instance</param>
        void DropTestingTransactionContexts(string namedInstance);


        /// <summary>
        /// Retrieves a dictionary of DbContextBase subclasses
        /// by named instance
        /// </summary>
        /// <param name="namedInstance">The GUID key for the 
        /// testing instance</param>
        /// <returns>a dictionary of DbContextBase classes
        /// keyed by the ConnectionStringName</returns>        
        Dictionary<string, DbContextBase> GetDbContexts(string namedInstance);

    }
}