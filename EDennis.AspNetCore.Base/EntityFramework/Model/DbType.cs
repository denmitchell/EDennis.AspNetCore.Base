namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This enum is used to characterize the 
    /// nature of the Database or connection
    /// (useful in testing scenarios)
    /// </summary>
    public enum DbType {
        InMemory,
        Transaction,
        Default
    }
}
