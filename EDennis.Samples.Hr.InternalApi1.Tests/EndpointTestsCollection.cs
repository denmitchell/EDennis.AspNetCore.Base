using EDennis.Samples.Hr.InternalApi1;
using Xunit;

namespace EDennis.AspNetCore.Base.Testing {

    [CollectionDefinition("Endpoint Tests")]
    public class EndpointTestsCollection 
        : EndpointTestsCollectionBase<Startup>{
    }
}
