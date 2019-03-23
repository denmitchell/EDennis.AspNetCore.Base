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
        : WriteableTemporalIntegrationTests<Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public EmployeeControllerIntegrationTests(ITestOutputHelper output, 
                ConfiguringWebApplicationFactory<Startup> factory)
            : base(output, factory) { }


        [Theory]
        [TestJson("EmployeeController", "CreateEmployee", "IntegrationTests", "Moe", testJsonConfigPath: "TestJsonConfigs\\Hr.json")]
        [TestJson("EmployeeController", "CreateEmployee", "IntegrationTests", "Larry", testJsonConfigPath: "TestJsonConfigs\\Hr.json")]
        [TestJson("EmployeeController", "CreateEmployee", "IntegrationTests", "Curly", testJsonConfigPath: "TestJsonConfigs\\Hr.json")]
        public void CreateEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var input = jsonTestCase.GetObject<Employee>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected").OrderBy(x=>x.Id);

            HttpClient.Post("api/employee", input);
            var actual = HttpClient.Get<Employee>($"api/employee/{id}").Value;

            Assert.True(actual.IsEqualOrWrite(expected,PROPS_FILTER,Output));
        }


        [Theory]
        [TestJson("EmployeeController", "GetEmployee", "IntegrationTests", "1", testJsonConfigPath: "TestJsonConfigs\\Hr.json")]
        [TestJson("EmployeeController", "GetEmployee", "IntegrationTests", "2", testJsonConfigPath: "TestJsonConfigs\\Hr.json")]
        [TestJson("EmployeeController", "GetEmployee", "IntegrationTests", "3", testJsonConfigPath: "TestJsonConfigs\\Hr.json")]
        [TestJson("EmployeeController", "GetEmployee", "IntegrationTests", "4", testJsonConfigPath: "TestJsonConfigs\\Hr.json")]
        public void CreateChecks(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = HttpClient.Get<Employee>($"api/employee/{id}").Value;

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

    }
}
