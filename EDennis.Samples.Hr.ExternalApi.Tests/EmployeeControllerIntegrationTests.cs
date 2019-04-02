using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Hr.ExternalApi.Models;
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


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Hr", "EDennis.Samples.Hr.ExternalApi", "EmployeeController", methodName, testScenario, testCase) {
            }
        }


        [Theory]
        [TestJson_("CreateEmployee", "IntegrationTests", "Moe")]
        [TestJson_("CreateEmployee", "IntegrationTests", "Larry")]
        [TestJson_("CreateEmployee", "IntegrationTests", "Curly")]
        public void CreateEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var input = jsonTestCase.GetObject<Employee>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            HttpClient.Post("api/employee", input);
            var actual = HttpClient.Get<Employee>($"api/employee/{id}").Value;

            Assert.True(actual.IsEqualOrWrite(expected,PROPS_FILTER,Output));
        }


        [Theory]
        [TestJson_("GetEmployee", "IntegrationTests", "1")]
        [TestJson_("GetEmployee", "IntegrationTests", "2")]
        [TestJson_("GetEmployee", "IntegrationTests", "3")]
        [TestJson_("GetEmployee", "IntegrationTests", "4")]
        public void GetEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = HttpClient.Get<Employee>($"api/employee/{id}").Value;

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

    }
}
