using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// Used for OAuth/OIDC authentication.
    /// NOTE: This class requires an entry in the Apis section
    /// of Configuration.  The key should be "Apis:IdentityServerApi".
    /// If a different configuration key is needed, extend the class
    /// with a different class name. 
    /// </summary>
    public class IdentityServerApi : ApiClient {
        public IdentityServerApi(HttpClient httpClient, IConfiguration config, ScopeProperties scopeProperties) : base(httpClient, config, scopeProperties) {
        }
    }
}
