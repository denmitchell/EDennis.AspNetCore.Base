using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi2 : ApiClient {

        private const string AGENCY_INVESTIGATOR_URL = "iapi/AgencyInvestigatorCheck";
        private const string AGENCY_ONLINE_URL = "iapi/AgencyOnlineCheck";
        private const string FEDERAL_BACKGROUND_URL = "iapi/FederalBackgroundCheck";
        private const string STATE_BACKGROUND_URL = "iapi/StateBackgroundCheck";
        private const string PREEMPLOYMENT_URL = "iapi/PreEmployment";


        public InternalApi2(HttpClient client, IConfiguration config, TestHeader testHeader) :
            base(client, config, testHeader) {
        }

        public void CreateAgencyOnlineCheck(AgencyOnlineCheck check) 
            => HttpClient.Post(AGENCY_ONLINE_URL, check);

        public void CreateAgencyInvestigatorCheck(AgencyInvestigatorCheck check) 
            => HttpClient.Post(AGENCY_INVESTIGATOR_URL, check);

        public dynamic GetPreEmploymentChecks(int employeeId)
            => HttpClient.Get<dynamic>($"{AGENCY_ONLINE_URL}/{employeeId}");

    }
}
