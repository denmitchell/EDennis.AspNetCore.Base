using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace EDennis.Samples.Hr.ExternalApi {

    public class InternalApi1 : ApiClient {

        private string _employeeControllerUrl = "iapi/color";

        public InternalApi1(HttpClient client, IConfiguration config) :
            base(client, config) {
            _employeeControllerUrl = config["Apis:InternalApi1:ControllerUrls:EmployeeController"];
        }

        public void CreateEmployee(Employee employee) {
            HttpClient.Post(_employeeControllerUrl, employee);
        }

        public void UpdateEmployee(Employee employee, int employeeId) {
            HttpClient.Put($"{_employeeControllerUrl}\\{employeeId}", employee);
        }

        public void DeleteEmployee(int employeeId) {
            HttpClient.Delete<Employee>($"{_employeeControllerUrl}\\{employeeId}");
        }

        public Employee GetEmployee(int employeeId) {
            var emp = HttpClient.Get<Employee>($"{_employeeControllerUrl}\\{employeeId}");
            return emp.Value;
        }
    }
}
