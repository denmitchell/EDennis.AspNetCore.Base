using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Abstractions;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi2 : PrivilegedApiClient {


        private const string AGENCY_INVESTIGATOR_URL = "iapi/AgencyInvestigatorCheck";
        private const string AGENCY_ONLINE_URL = "iapi/AgencyOnlineCheck";
        private const string FEDERAL_BACKGROUND_URL = "iapi/FederalBackgroundCheck";
        private const string STATE_BACKGROUND_URL = "iapi/StateBackgroundCheck";
        private const string PREEMPLOYMENT_URL = "iapi/PreEmployment";


        public InternalApi2(HttpClient client,
            IdentityServer identityServer,
            IConfiguration config,
            ScopeProperties scopeProperties,
            SecureTokenCache tokenCache,
            IHostingEnvironment env
            ) :
            base(client, identityServer,
                config, scopeProperties,
                tokenCache, env) {
        }


        public ObjectResult<AgencyOnlineCheck> CreateAgencyOnlineCheck(AgencyOnlineCheck check)
                => HttpClient.Post(AGENCY_ONLINE_URL, check);

        public ObjectResult<AgencyInvestigatorCheck> CreateAgencyInvestigatorCheck(AgencyInvestigatorCheck check)
            => HttpClient.Post(AGENCY_INVESTIGATOR_URL, check);

        public ObjectResult<dynamic> GetPreEmploymentChecks(int employeeId)
            => HttpClient.Get<dynamic>($"{AGENCY_ONLINE_URL}/{employeeId}");

    }
}
