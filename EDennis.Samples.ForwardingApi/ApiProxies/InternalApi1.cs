using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.ForwardingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace EDennis.Samples.ForwardingApi {

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


        public Employee UpdateEmployee(Employee employee, int id) {
            var emp = HttpClient.Put(employee, id);
            return emp;
        }


        public void DeleteEmployee(int employeeId) {
            HttpClient.Delete(employeeId);
        }


        public Employee GetEmployee(int employeeId) {
            var emp = HttpClient.Get<Employee>(employeeId);
            return emp;
        }

        public HttpResponseMessage Forward(HttpContext context, int id) {
            var url = HttpClient.BaseAddress.At(id).ToString();
            var response = HttpClient.Forward(context, url);
            return response;
        }

        public HttpResponseMessage Forward(HttpContext context) {
            var url = HttpClient.BaseAddress.ToString();
            var response = HttpClient.Forward(context, url);
            return response;
        }


    }
}
