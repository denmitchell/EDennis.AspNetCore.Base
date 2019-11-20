using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Web {
    public interface IApiClient {
        Api Api { get; set; }
        string ApiKey { get; set; }
        HttpClient HttpClient { get; set; }
        ILogger Logger { get; }
        IScopeProperties ScopeProperties { get; set; }
    }
}