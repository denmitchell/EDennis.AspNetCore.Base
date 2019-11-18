using EDennis.Samples.Colors.InternalApi;
using Xunit;

namespace EDennis.AspNetCore.Base.Testing {

    [CollectionDefinition("Internal Endpoint Tests")]
    public class InternalEndpointTestsCollection 
        //: ICollectionFixture<ConfiguringWebApplicationFactory<Startup>> { 
        : EndpointTestsCollectionBase<Startup>{
    }
}
