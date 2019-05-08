using EDennis.Samples.Colors.ExternalApi;
using EDennis.Samples.Colors.InternalApi;
using Xunit;

namespace EDennis.AspNetCore.Base.Testing {
    [CollectionDefinition("ApiClient Tests")]
    public class ApiClientTestsCollection 
        : ApiClientTestsCollectionBase<InternalApi, Samples.Colors.InternalApi.Startup> {
    }
}