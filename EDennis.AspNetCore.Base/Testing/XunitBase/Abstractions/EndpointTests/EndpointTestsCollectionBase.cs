using Xunit;

namespace EDennis.AspNetCore.Base.Testing {
    /// <summary>
    /// Special note: each project that uses this CollectionDefinition
    /// must extend this class and resolve the TStartup type parameter
    /// to a concrete Startup.cs class -- the entry point for the
    /// test.  Test classes that use this CollectionDefinition
    /// must be decorated with [Collection("Endpoint Tests")]
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public abstract class EndpointTestsCollectionBase<TStartup> 
        : ICollectionFixture<ConfiguringWebApplicationFactory<TStartup>> 
        where TStartup: class{
    }
}
