using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// Used for OAuth/OIDC authentication.
    /// NOTE: This class requires an entry in the Apis section
    /// of Configuration.  The key should be "Apis:IdentityServerApi".
    /// If a different configuration key is needed, extend the class
    /// with a different class name. 
    /// </summary>
    public class IdentityServerApi : ApiClient, IIdentityServerApi {
        public IdentityServerApi(IHttpClientFactory httpClientFactory, 
            IOptionsMonitor<Apis> apis, 
            IScopeProperties scopeProperties, 
            IWebHostEnvironment env,
            ILogger<IdentityServerApi> logger) 
            : base(httpClientFactory, apis, scopeProperties, env, logger) {
        }
    }
}
