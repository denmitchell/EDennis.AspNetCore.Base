namespace EDennis.AspNetCore.Base.EntityFramework {
    /// <summary>
    /// All entities that have a key backed by
    /// a sequence or identity field are expected
    /// to implement this interface.
    /// </summary>
    public interface IHasLongId {
        long Id { get; set; } //primary key field
    }
}
