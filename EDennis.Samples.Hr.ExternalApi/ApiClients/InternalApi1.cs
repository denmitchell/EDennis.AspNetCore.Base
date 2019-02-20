using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi1 : ApiClient {

        private const string EMPLOYEE_URL = "iapi/employee";

        public InternalApi1(HttpClient client, IConfiguration config, TestHeader testHeader) :
            base(client, config, testHeader) {
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
