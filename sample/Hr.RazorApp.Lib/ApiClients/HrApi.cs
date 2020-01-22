using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Hr.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
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


        public async Task<ObjectResult> GetPersonsAsync(string where = null,
            string orderBy = null, string select = null, int? skip = null,
            int? take = null, int? totalRecords = null
            ) {
    
            var url = $"{PERSON_URL}{GetQueryString(where, orderBy, select, skip, take, totalRecords)}";           
            return await HttpClient.GetAsync<PagedResult<dynamic>,Person>(url);
        }

        public async Task<ObjectResult> GetPersonDetailsAsync(int? id) {
            return await HttpClient.GetAsync<Person>($"{PERSON_URL}/{id}");
        }

        public async Task<ObjectResult> CreatePersonAsync(Person person) {
            return await HttpClient.PostAsync($"{PERSON_URL}", person);
        }

        public async Task<ObjectResult> EditPersonAsync(Person person, int id) {
            return await HttpClient.PutAsync($"{PERSON_URL}/{id}", person);
        }

        public async Task<StatusCodeResult> DeletePersonAsync(int id) {
            return await HttpClient.DeleteAsync<Person>($"{PERSON_URL}/{id}");
        }


        public async Task<ObjectResult> GetAddressesAsync(string where = null,
            string orderBy = null, string select = null, int? skip = null,
            int? take = null, int? totalRecords = null
            ) {

            var url = $"{ADDRESS_URL}{GetQueryString(where, orderBy, select, skip, take, totalRecords)}";
            return await HttpClient.GetAsync<PagedResult<dynamic>, Address>(url);
        }


        public async Task<ObjectResult> GetAddressDetailsAsync(int? id) {
            return await HttpClient.GetAsync<Address>($"{ADDRESS_URL}/{id}");
        }

        public async Task<ObjectResult> CreateAddressAsync(Address address) {
            return await HttpClient.PostAsync($"{ADDRESS_URL}", address);
        }

        public async Task<ObjectResult> EditAddressAsync(Address address, int id) {
            return await HttpClient.PutAsync($"{ADDRESS_URL}/{id}", address);
        }

        public async Task<StatusCodeResult> DeleteAddressAsync(int id) {
            return await HttpClient.DeleteAsync<Address>($"{ADDRESS_URL}/{id}");
        }



        private string GetQueryString(string where = null,
            string orderBy = null, string select = null, int? skip = null,
            int? take = null, int? totalRecords = null) {

            var q = new List<string>();
            if (!string.IsNullOrWhiteSpace(where))
                q.Add($"where={where}");
            if (!string.IsNullOrWhiteSpace(orderBy))
                q.Add($"orderBy={orderBy}");
            if (!string.IsNullOrWhiteSpace(select))
                q.Add($"select={select}");
            if (skip != null)
                q.Add($"skip={skip.Value}");
            if (take != null)
                q.Add($"take={take}");
            if (totalRecords != null)
                q.Add($"totalRecords={totalRecords}");

            var qString = "?" + string.Join('&', q);

            return qString;
        }


    }
}
