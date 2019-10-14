using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Abstractions;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi1 : SecureApiClient {

        private const string EMPLOYEE_URL = "iapi/employee";

        public InternalApi1(
            HttpClient client, 
            IConfiguration config, 
            ScopeProperties scopeProperties,
            IdentityServerApi identityServer,
            SecureTokenCache tokenCache,
            IHostingEnvironment env
            ) :
            base(client, config, scopeProperties,
                identityServer,tokenCache, env) {
        }

        public ObjectResult CreateEmployee(Employee employee) {
            var response = HttpClient.Post(EMPLOYEE_URL, employee);
            return response;
        }

        public ObjectResult UpdateEmployee(Employee employee, int id) {
            var response = HttpClient.Put($"{EMPLOYEE_URL}/{id}", employee);
            return response;
        }

        public HttpStatusCode DeleteEmployee(int id) {
            var response = HttpClient.Delete<Employee>($"{EMPLOYEE_URL}/{id}");
            return (HttpStatusCode)response.StatusCode;
        }

        public ObjectResult GetEmployee(int id) {
            var response = HttpClient.Get<Employee>($"{EMPLOYEE_URL}/{id}");
            return response;
        }

        public ObjectResult GetEmployees(int pageNumber, int pageSize) {
            var response = HttpClient.Get<List<Employee>>($"{EMPLOYEE_URL}");
            return response;
        }

    }
}
