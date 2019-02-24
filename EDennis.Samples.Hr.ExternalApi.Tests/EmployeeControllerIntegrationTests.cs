using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.ExternalApi.Tests {

    public class EmployeeControllerIntegrationTests
        : WriteableTemporalIntegrationTests<EDennis.Samples.Hr.ExternalApi.Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public EmployeeControllerIntegrationTests(ITestOutputHelper output, WebApplicationFactory<EDennis.Samples.Hr.ExternalApi.Startup> factory)
            : base(output, factory) { }


        [Theory]
        [TestJson("EmployeeController", "CreateEmployee", "IntegrationTests", "Moe", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateEmployee", "IntegrationTests", "Larry", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateEmployee", "IntegrationTests", "Curly", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        public void CreateEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected").OrderBy(x=>x.Id);

            HttpClient.Post("api/employee", input);
            var actual = HttpClient.Get<List<Employee>>("api/employee").Value.OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected,PROPS_FILTER,Output));
        }


        [Theory]
        [TestJson("EmployeeController", "CreateChecks", "IntegrationTests", "1", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateChecks", "IntegrationTests", "2", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateChecks", "IntegrationTests", "3", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateChecks", "IntegrationTests", "4", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        public void CreateChecks(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var input = jsonTestCase.GetObject<Employee>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<dynamic>("Expected");

            HttpClient.Post("api/employee",input);
            var actual = HttpClient.Get<dynamic>($"preemployment/{id}").Value;

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

    }
}
