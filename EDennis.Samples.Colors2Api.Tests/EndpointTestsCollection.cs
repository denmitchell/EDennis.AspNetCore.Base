using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.Colors2Api;
using Xunit;

namespace EDennis.Samples.Colors2Api.Tests {

    [CollectionDefinition("Endpoint Tests")]
    public class EndpointTestsCollection 
        : EndpointTestsCollectionBase<Startup>{
    }
}
