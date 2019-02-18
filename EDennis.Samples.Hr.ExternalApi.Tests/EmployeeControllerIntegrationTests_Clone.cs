using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.ExternalApi.Tests {

    public class EmployeeControllerIntegrationTests_Clone
        : CloneIntegrationTests<EDennis.Samples.Hr.ExternalApi.Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public EmployeeControllerIntegrationTests_Clone(ITestOutputHelper output, CloneWebApplicationFactory<EDennis.Samples.Hr.ExternalApi.Startup> factory)
            : base(output, factory) { }


        [Theory]
        [TestJson(className: "EmployeeController", methodName: "CreateEmployee", testScenario: "MultitierIntegrationTests_InMemory", testCase: "Moe")]
        [TestJson(className: "EmployeeController", methodName: "CreateEmployee", testScenario: "MultitierIntegrationTests_InMemory", testCase: "Larry")]
        [TestJson(className: "EmployeeController", methodName: "CreateEmployee", testScenario: "MultitierIntegrationTests_InMemory", testCase: "Curly")]
        public void CreateEmployee(string firstName) {
            var newEmployee = new Employee {
                FirstName = firstName
            };
            _client.Post("api/employee", newEmployee);
            var actual = _client.Get<Employee>("api/employee").Value;
            Assert.Equal(firstName, actual.FirstName);
        }


        [Theory]
        [TestJson(className: "EmployeeController", methodName: "CreateChecks", testScenario: "MultitierIntegrationTests_InMemory", testCase: "1")]
        [TestJson(className: "EmployeeController", methodName: "CreateChecks", testScenario: "MultitierIntegrationTests_InMemory", testCase: "2")]
        [TestJson(className: "EmployeeController", methodName: "CreateChecks", testScenario: "MultitierIntegrationTests_InMemory", testCase: "3")]
        [TestJson(className: "EmployeeController", methodName: "CreateChecks", testScenario: "MultitierIntegrationTests_InMemory", testCase: "4")]
        public void CreateChecks(int employeeId, 
            string dateCompletedString, string status) {

            var dateCompleted = DateTime.Parse(dateCompletedString);

            var newCheck = new AgencyInvestigatorCheck {
                EmployeeId = employeeId,
                DateCompleted = dateCompleted,
                Status = status
            };


            _client.Post("api/employee",newCheck);
            var actual = _client.Get<AgencyInvestigatorCheck>($"preemployment/{employeeId}").Value;

            var actualJson = JToken.FromObject(actual).ToString();
            _output.WriteLine(actualJson);
            //Assert.Equal(expectedJson, actualJson);
        }

    }
}
