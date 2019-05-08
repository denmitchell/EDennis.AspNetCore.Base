using EDennis.Samples.Colors.ExternalApi;
using Xunit;

namespace EDennis.AspNetCore.Base.Testing {
    [CollectionDefinition("External Endpoint Tests")]
    public class ExternalEndpointTestsCollection 
        : EndpointTestsCollectionBase<Startup> {
    }
}