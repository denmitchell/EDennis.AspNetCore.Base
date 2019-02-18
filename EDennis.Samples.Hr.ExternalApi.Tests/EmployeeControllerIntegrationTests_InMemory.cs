using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.ExternalApi.Tests {

    public class EmployeeControllerIntegrationTests_InMemory
        : InMemoryIntegrationTests<EDennis.Samples.Hr.ExternalApi.Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public EmployeeControllerIntegrationTests_InMemory(ITestOutputHelper output, InMemoryWebApplicationFactory<EDennis.Samples.Hr.ExternalApi.Startup> factory)
            : base(output, factory) { }


        [Theory]
        [InlineData("Bob")]
        [InlineData("Drew")]
        [InlineData("Moe")]
        [InlineData("Larry")]
        [InlineData("Curly")]
        public void CreateEmployee(string firstName) {
            var newEmployee = new Employee {
                FirstName = firstName
            };
            _client.Post("api/employee", newEmployee);
            var actual = _client.Get<Employee>("api/employee").Value;
            Assert.Equal(firstName, actual.FirstName);
        }


        [Theory]
        [InlineData(1, "2018-12-01", "Pass")]
        [InlineData(2, "2018-12-02", "Fail")]
        [InlineData(3, "2018-12-03", "Pass")]
        [InlineData(4, "2018-12-04", "Fail")]
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
