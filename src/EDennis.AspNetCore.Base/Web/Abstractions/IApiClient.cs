using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Web {
    public interface IApiClient {
        Api Api { get; }
        string ApiKey { get; }
        HttpClient HttpClient { get; }
        IScopeProperties ScopeProperties { get; }
    }
}