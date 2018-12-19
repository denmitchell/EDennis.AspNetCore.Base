namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This enum is used to characterize the 
    /// nature of the Database or connection
    /// (useful in testing scenarios)
    /// </summary>
    public enum DbType {
        InMemory, //in-memory database (for testing)
        Transaction, //SQL Server database with an open transaction that is managed by application/testing code
        Default //SQL database where transactions are managed by entity framework
    }
}
