using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.ExternalApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace EDennis.Samples.ExternalApi {

    public class InternalApi1 : IApiProxy {

        public HttpClient HttpClient { get; set; }
        public Uri ResetUri { get => HttpClient.BaseAddress; }

        public InternalApi1(HttpClient client, IConfiguration config ) {
            HttpClient = client;
            
            HttpClient.BaseAddress = new Uri(
                client.BaseAddress + this.GetControllerUrl(config, "EmployeeController"));
        }


        public Employee CreateEmployee(Employee employee) {
            var response = HttpClient.PostAsync(HttpClient.BaseAddress, new BodyContent<Employee>(employee));
            var message = response.Result;
            var emp = message.GetObject<Employee>();
            return emp;
        }

        public Employee GetEmployee(int employeeId) {
            var response = HttpClient.GetAsync($"{HttpClient.BaseAddress}/{employeeId}");
            var message = response.Result;
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            var emp = message.GetObject<Employee>();
            return emp;
        }
    }
}
