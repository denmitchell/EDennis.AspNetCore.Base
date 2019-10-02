using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EDennis.Samples.DefaultPoliciesMvc.ApiClients {
    public class IdentityServerClient : ApiClient {
        public IdentityServerClient(HttpClient client, IConfiguration config,
            ScopeProperties scopeProperties) :
            base(client, config, scopeProperties) { }

    }
}
