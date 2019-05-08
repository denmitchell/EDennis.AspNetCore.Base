using EDennis.AspNetCore.Base.Web;
using Xunit;

namespace EDennis.AspNetCore.Base.Testing {
    /// <summary>
    /// Special note: each project that uses this CollectionDefinition
    /// must extend this class and resolve the TStartup type parameter
    /// to a concrete Startup.cs class -- the entry point for the
    /// test.  Test classes that use this CollectionDefinition
    /// must be decorated with [Collection("ApiClient Tests")]
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public abstract class ApiClientTestsCollectionBase<TClient,TStartup> : ICollectionFixture<ApiLauncherFactory<TStartup>> 
        where TStartup: class
        where TClient : ApiClient {
    }
}
