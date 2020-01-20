using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Hr.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hr.RazorApp {
    public class HrApi : SecureApiClient {

        private const string PERSON_URL = "api/Person";
        private const string ADDRESS_URL = "api/Address";

        public HrApi(IHttpClientFactory httpClientFactory, IOptionsMonitor<Apis> apis, IScopeProperties scopeProperties, ISecureTokenService secureTokenService, IWebHostEnvironment env, ILogger logger) 
            : base(httpClientFactory, apis, scopeProperties, secureTokenService, env, logger) {
        }

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


        public ObjectResult GetAddresses() {
            return HttpClient.Get<List<Address>>(ADDRESS_URL);
        }

        public ObjectResult GetAddressDetails(int? id) {
            return HttpClient.Get<Address>($"{ADDRESS_URL}/{id}");
        }

        public ObjectResult CreateAddress(Address person) {
            return HttpClient.Post($"{ADDRESS_URL}", person);
        }

        public ObjectResult EditAddress(int id, Address person) {
            return HttpClient.Put($"{ADDRESS_URL}/{id}", person);
        }

        public StatusCodeResult DeleteAddress(int id) {
            return HttpClient.Delete<Address>($"{ADDRESS_URL}/{id}");
        }



    }
}
