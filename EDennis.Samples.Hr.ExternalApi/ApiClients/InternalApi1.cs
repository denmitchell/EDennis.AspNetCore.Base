using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Abstractions;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi1 : SecureApiClient {

        private const string EMPLOYEE_URL = "iapi/employee";

        public InternalApi1(
            HttpClient client, 
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

        public void CreateEmployee(Employee employee) {
            HttpClient.Post(EMPLOYEE_URL, employee);
        }

        public void UpdateEmployee(Employee employee, int id) {
            HttpClient.Put($"{EMPLOYEE_URL}/{id}", employee);
        }

        public void DeleteEmployee(int id) {
            HttpClient.Delete<Employee>($"{EMPLOYEE_URL}/{id}");
        }

        public Employee GetEmployee(int id) {
            var emp = HttpClient.Get<Employee>($"{EMPLOYEE_URL}/{id}");
            return emp.Value;
        }

        public List<Employee> GetEmployees(int pageNumber, int pageSize) {
            var emps = HttpClient.Get<List<Employee>>($"{EMPLOYEE_URL}");
            return emps.Value;
        }

    }
}
