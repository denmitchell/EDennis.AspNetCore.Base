using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class IdentityServer : ApiClient {

        public IdentityServer(HttpClient client, IConfiguration config, ScopeProperties scopeProperties) :
            base(client, config, scopeProperties) {
        }

    }
}
