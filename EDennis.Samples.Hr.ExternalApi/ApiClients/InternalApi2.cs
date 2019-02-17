using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi2 : ApiClient {

        private string _agencyInvestigatorCheckControllerUrl = "iapi/AgencyInvestigatorCheck";
        private string _agencyOnlineCheckControllerUrl = "iapi/AgencyOnlineCheck";
        private string _federalBackgroundCheckControllerUrl = "iapi/FederalBackgroundCheck";
        private string _stateBackgroundCheckControllerUrl = "iapi/StateBackgroundCheck";
        private string _preEmploymentControllerUrl = "iapi/PreEmployment";


        public InternalApi2(HttpClient client, IConfiguration config) :
            base(client, config) {
            _agencyInvestigatorCheckControllerUrl = config["Apis:InternalApi2:ControllerUrls:AgencyInvestigatorCheckController"];
            _agencyOnlineCheckControllerUrl = config["Apis:InternalApi2:ControllerUrls:AgencyOnlineCheckController"];
            _federalBackgroundCheckControllerUrl = config["Apis:InternalApi2:ControllerUrls:FederalBackgroundCheckController"];
            _stateBackgroundCheckControllerUrl = config["Apis:InternalApi2:ControllerUrls:StateBackgroundCheckController"];
            _preEmploymentControllerUrl = config["Apis:InternalApi2:ControllerUrls:PreEmploymentController"];
        }

        public void CreateAgencyOnlineCheck(AgencyOnlineCheck check) 
            => HttpClient.Post(_agencyOnlineCheckControllerUrl, check);

        public void CreateAgencyInvestigatorCheck(AgencyInvestigatorCheck check) 
            => HttpClient.Post(_agencyInvestigatorCheckControllerUrl, check);

        public dynamic GetPreEmploymentChecks(int employeeId)
            => HttpClient.Get<dynamic>($"{_agencyOnlineCheckControllerUrl}/{employeeId}");

    }
}
