using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi1 : IApiProxy {

        public HttpClient HttpClient { get; set; }
        public Uri ResetUri { get => HttpClient.BaseAddress; }

        public InternalApi1(HttpClient client, IConfiguration config ) {
            HttpClient = client;
            
            HttpClient.BaseAddress = new Uri(
                client.BaseAddress + this.GetControllerUrl(config, "EmployeeController"));
        }


        public Employee CreateEmployee(Employee employee) {
            var emp = HttpClient.Post(employee);
            return emp;
        }

        public Employee UpdateEmployee(Employee employee, int employeeId) {
            var emp = HttpClient.Put(employee, employeeId);
            return emp;
        }

        public void DeleteEmployee(int employeeId) {
            HttpClient.Delete(employeeId);
        }

        public Employee GetEmployee(int employeeId) {
            var emp = HttpClient.Get<Employee>(employeeId);
            return emp;
        }
    }
}
