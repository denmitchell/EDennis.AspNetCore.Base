using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.ForwardingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task<Employee> CreateEmployeeAsync(HttpRequest httpRequest, Employee employee) {
            var url = HttpClient.BaseAddress.ToString();
            var response = await HttpClient.ForwardRequestAsync(httpRequest,employee,url);
            var emp = await response.GetObjectAsync<Employee>();
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

        public async Task<Employee> GetEmployeeAsync(HttpRequest httpRequest, int employeeId) {
            var url = HttpClient.BaseAddress.At(employeeId).ToString();
            var response = await HttpClient.ForwardRequestAsync(httpRequest, url);
            var emp = await response.GetObjectAsync<Employee>();
            return emp;
        }

    }
}
