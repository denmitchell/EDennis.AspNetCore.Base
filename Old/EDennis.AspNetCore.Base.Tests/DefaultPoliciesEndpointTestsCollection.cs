using EDennis.Samples.DefaultPoliciesApi;
using Xunit;

namespace EDennis.AspNetCore.Base.Testing {

    [CollectionDefinition("DefaultPolicies Endpoint Tests")]
    public class DefaultPoliciesEndpointTestsCollection 
        //: ICollectionFixture<ConfiguringWebApplicationFactory<Startup>> { 
        : EndpointTestsCollectionBase<Startup>{
    }
}
