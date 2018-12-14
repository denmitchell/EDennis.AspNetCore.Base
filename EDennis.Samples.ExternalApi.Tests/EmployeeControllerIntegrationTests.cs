using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using EDennis.AspNetCore.Testing;
using EDennis.Samples.ExternalApi.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace EDennis.Samples.ExternalApi.Tests {

    [Collection("Sequential")]
    public class EmployeeControllerIntegrationTests
        : IClassFixture<WebApplicationFactory<Startup>> {

        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;
        private const int NEXTID = 5;

        public EmployeeControllerIntegrationTests(
                WebApplicationFactory<Startup> factory,
                ITestOutputHelper output) {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5999/api/employee",UriKind.Absolute);
            _output = output;
        }

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
            var actual = _client.PostAndGet(newEmployee,NEXTID);
            Assert.Equal(firstName, actual.FirstName);
        }

        [Theory]
        [InlineData(1, "2018-12-01", "Pass", "{}")]
        [InlineData(2, "2018-12-02", "Fail", "{}")]
        [InlineData(3, "2018-12-03", "Pass", "{}")]
        [InlineData(4, "2018-12-04", "Fail", "{}")]
        public void CreateChecks(int employeeId, 
            string dateCompletedString, string status,
            string expectedJson) {

            var dateCompleted = DateTime.Parse(dateCompletedString);

            var newCheck = new AgencyInvestigatorCheck {
                EmployeeId = employeeId,
                DateCompleted = dateCompleted,
                Status = status
            };
            _client.BaseAddress = new Uri("http://localhost:5999/api/employee/", UriKind.Absolute);

            var actual = _client.PostAndGet(newCheck, DbType.Default, "default",
                getUri: new Uri(_client.BaseAddress + $"preemployment/{employeeId}"),
                postUri: new Uri(_client.BaseAddress + "agencyinvestigatorcheck")
                );

            var actualJson = JToken.FromObject(actual).ToString();
            _output.WriteLine(actualJson);
            //Assert.Equal(expectedJson, actualJson);
        }

    }
}
