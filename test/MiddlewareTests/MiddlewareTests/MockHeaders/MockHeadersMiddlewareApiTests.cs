using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Lib = EDennis.Samples.MockHeadersMiddlewareApi.Lib;
using Lcr = EDennis.Samples.MockHeadersMiddlewareApi.Launcher;
using EDennis.AspNetCore.Base.Testing;

namespace EDennis.AspNetCore.MiddlewareTests {
    /// <summary>
    /// Note: IClassFixture was not used because
    /// the individual test cases were conflicting with each
    /// (possibly, one test case was updating the configuration
    /// while another test case was calling Get).  To resolve
    /// the issue, I instantiate the TestApis within the
    /// test method.  This is inefficient, but it works.
    /// </summary>
    [Collection("Sequential")]
    public class MockHeadersMiddlewareApiTests {

        private readonly ITestOutputHelper _output;

        public MockHeadersMiddlewareApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("MockHeadersApi", "MockHeadersController",
                      methodName, testScenario, testCase, NetCoreTestingUtilities.DatabaseProvider.Excel, "MockHeaders\\TestJson.xlsx") {
            }
        }


        [Theory]
        [TestJsonA("Get", "", "A")]
        [TestJsonA("Get", "", "B")]
        [TestJsonA("Get", "", "C")]
        public void Get(string t, JsonTestCase jsonTestCase) {

            using var fixture = new LauncherFixture<Lib.Program, Lcr.Program>();
            var client = fixture.HttpClient;

            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"MockHeaders\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var headersQueryString = jsonTestCase.GetObject<string>("Headers");
            var expected = jsonTestCase.GetObject<List<KeyValuePair<string,string>>>("Expected");

            var url = $"Headers?{headersQueryString}";

            var result = client.GetAsync(client.BaseAddress.ToString() + url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            _output.WriteLine(content);

            var actual = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(content);

            Assert.True(actual.IsEqualOrWrite(expected, _output, true));
        }

    }
}
