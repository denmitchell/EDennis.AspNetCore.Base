namespace EDennis.AspNetCore.Base.EntityFramework {
    /// <summary>
    /// This interface is used by IServiceCollectionExtensions
    /// to reflectively build a list of repository classes and
    /// configure their dependency injection.  WriteableRepo
    /// and QueryableRepo both implement this interface.
    /// </summary>
    public interface IRepo { }
}
