using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Abstractions;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi2 : SecureApiClient {


        private const string AGENCY_INVESTIGATOR_URL = "iapi/AgencyInvestigatorCheck";
        private const string AGENCY_ONLINE_URL = "iapi/AgencyOnlineCheck";


        public InternalApi2(HttpClient client,
            IConfiguration config,
            ScopeProperties22 scopeProperties,
            IdentityServerApi identityServer,
            SecureTokenCache tokenCache,
            IWebHostEnvironment env, ILogger<InternalApi2> logger
            ) :
            base(client, config, scopeProperties,
                identityServer, tokenCache, env, logger) {
        }


        public ObjectResult CreateAgencyOnlineCheck(AgencyOnlineCheck check)
                => HttpClient.Post(AGENCY_ONLINE_URL, check);

        public ObjectResult CreateAgencyInvestigatorCheck(AgencyInvestigatorCheck check)
            => HttpClient.Post(AGENCY_INVESTIGATOR_URL, check);

        public ObjectResult GetPreEmploymentChecks(int employeeId)
            => HttpClient.Get<dynamic>($"{AGENCY_ONLINE_URL}/{employeeId}");

    }
}
