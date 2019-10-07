using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Abstractions;
using EDennis.Samples.DefaultPoliciesMvc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.DefaultPoliciesMvc.ApiClients {

    public class DefaultPoliciesApiClient : SecureApiClient, IDefaultPoliciesApiClient {

        private const string PERSON_URL = "api/Person";
        private const string POSITION_URL = "api/Position";

        public DefaultPoliciesApiClient(HttpClient client, IConfiguration config,
            ScopeProperties scopeProperties, IdentityServerApi identityServerClient,
            SecureTokenCache secureTokenCache, IHostingEnvironment hostingEnvironment
            ) : base(client, config, scopeProperties,
                  identityServerClient,
                secureTokenCache, hostingEnvironment) { }


        public ObjectResult GetPersons() {
            return HttpClient.Get<List<Person>>(PERSON_URL);
        }

        public ObjectResult GetPersonDetails(int? id) {
            return HttpClient.Get<Person>($"{PERSON_URL}/{id}");
        }

        public ObjectResult CreatePerson(Person person) {
            return HttpClient.Post($"{PERSON_URL}", person);
        }

        public ObjectResult EditPerson(int id, Person person) {
            return HttpClient.Put($"{PERSON_URL}/{id}", person);
        }

        public StatusCodeResult DeletePerson(int id) {
            return HttpClient.Delete<Person>($"{PERSON_URL}/{id}");
        }

        public StatusCodeResult PersonExists(int id) {
            return HttpClient.GetStatusCodeResult($"{PERSON_URL}/Exists/{id}");
        }


        public ObjectResult GetPositions() {
            return HttpClient.Get<List<Position>>(POSITION_URL);
        }

        public ObjectResult GetPositionDetails(int? id) {
            return HttpClient.Get<Position>($"{POSITION_URL}/{id}");
        }

        public ObjectResult CreatePosition(Position person) {
            return HttpClient.Post($"{POSITION_URL}", person);
        }

        public ObjectResult EditPosition(int id, Position person) {
            return HttpClient.Put($"{POSITION_URL}/{id}", person);
        }

        public StatusCodeResult DeletePosition(int id) {
            return HttpClient.Delete<Position>($"{POSITION_URL}/{id}");
        }

        public StatusCodeResult PositionExists(int id) {
            return HttpClient.GetStatusCodeResult($"{POSITION_URL}/Exists/{id}");
        }

    }
}
