using EDennis.Samples.Hr.ExternalApi;
using Xunit;

namespace EDennis.AspNetCore.Base.Testing {

    [CollectionDefinition("Endpoint Tests")]
    public class EndpointTestsCollection 
        : EndpointTestsCollectionBase<Startup>{
    }
}
