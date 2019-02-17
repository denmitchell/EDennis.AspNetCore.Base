using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi2 : IApiProxy {

        public HttpClient HttpClient { get; set; }
        public Uri ResetUri { get; }

        private Dictionary<string, string> _urls
            = new Dictionary<string, string>();


        public InternalApi2(HttpClient client, IConfiguration config ) {
            HttpClient = client;

            void Add(string c) => 
                _urls.Add(c, client.BaseAddress 
                    + this.GetControllerUrl(config, $"{c}Controller"));

            Add("AgencyOnlineCheck");
            Add("AgencyInvestigatorCheck");
            Add("FederalBackgroundCheck");
            Add("StateBackgroundCheck");
            Add("PreEmployment");

            ResetUri = new Uri(_urls["AgencyOnlineCheck"]);
        }



        public AgencyOnlineCheck CreateAgencyOnlineCheck(AgencyOnlineCheck check) 
            => HttpClient.Post(check, new Uri(_urls["AgencyOnlineCheck"]));

        public AgencyInvestigatorCheck CreateAgencyInvestigatorCheck(AgencyInvestigatorCheck check) 
            => HttpClient.Post(check, new Uri(_urls["AgencyInvestigatorCheck"]));

        public dynamic GetPreEmploymentChecks(int employeeId)
            => HttpClient.Get<dynamic>(new Uri($"{_urls["PreEmployment"]}/{employeeId}"));

    }
}
