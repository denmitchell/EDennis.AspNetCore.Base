namespace EDennis.AspNetCore.Base.EntityFramework {
    /// <summary>
    /// All entities that have a key backed by
    /// a sequence or identity field are expected
    /// to implement this interface.
    /// </summary>
    public interface IHasIntegerId {
        int Id { get; set; } //primary key field
    }
}
