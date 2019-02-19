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

    public class EmployeeControllerIntegrationTests_InMemory
        : InMemoryIntegrationTests<EDennis.Samples.Hr.ExternalApi.Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public EmployeeControllerIntegrationTests_InMemory(ITestOutputHelper output, InMemoryWebApplicationFactory<EDennis.Samples.Hr.ExternalApi.Startup> factory)
            : base(output, factory) { }


        [Theory]
        [TestJson("EmployeeController", "CreateEmployee", "IntegrationTests", "Moe", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateEmployee", "IntegrationTests", "Larry", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateEmployee", "IntegrationTests", "Curly", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        public void CreateEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);
            _output.WriteLine($"Db instance name: {_instanceName}");

            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected").OrderBy(x=>x.Id);

            _client.Post("api/employee", input);
            var actual = _client.Get<List<Employee>>("api/employee").Value.OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected,PROPS_FILTER,_output));
        }


        [Theory]
        [TestJson("EmployeeController", "CreateChecks", "IntegrationTests", "1", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateChecks", "IntegrationTests", "2", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateChecks", "IntegrationTests", "3", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        [TestJson("EmployeeController", "CreateChecks", "IntegrationTests", "4", testJsonConfigPath: "TestJsonConfigs\\AgencyInvestigator.json")]
        public void CreateChecks(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);
            _output.WriteLine($"Db instance name: {_instanceName}");

            var input = jsonTestCase.GetObject<Employee>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<dynamic>("Expected");

            _client.Post("api/employee",input);
            var actual = _client.Get<dynamic>($"preemployment/{id}").Value;

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }

    }
}
