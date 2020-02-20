using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Lib = EDennis.Samples.ScopePropertiesMiddlewareApi.Lib;
using Lcr = EDennis.Samples.ScopePropertiesMiddlewareApi.Launcher;


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
    public class ScopePropertiesMiddlewareApiTests {

        private readonly ITestOutputHelper _output;

        public ScopePropertiesMiddlewareApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("ScopePropertiesApi", "ScopePropertiesController",
                      methodName, testScenario, testCase, NetCoreTestingUtilities.DatabaseProvider.Excel, "ScopeProperties\\TestJson.xlsx") {
            }
        }


        [Theory]
        [TestJsonA("Get", "", "A")]
        [TestJsonA("Get", "", "B")]
        [TestJsonA("Get", "", "C")]
        public void Get(string t, JsonTestCase jsonTestCase) {

            using var fixture = new LauncherFixture<Lib.Program, Lcr.Program>();
            var client = fixture.HttpClient;

            client.DefaultRequestHeaders.Add(Constants.ENTRYPOINT_KEY, "TestProject");

            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"ScopeProperties\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var claimsQueryString = jsonTestCase.GetObject<string>("Claims");
            var headersQueryString = jsonTestCase.GetObject<string>("Headers");
            var expected = jsonTestCase.GetObject<Dictionary<string, string>>("Expected");
            expected = expected.OrderBy(x => x.Key).ToDictionary(k => k.Key, v => v.Value);


            var url = $"ScopeProperties?{claimsQueryString}&{headersQueryString}";

            var result = client.GetAsync(client.BaseAddress.ToString() + url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            _output.WriteLine(content);

            var actual = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
            actual = actual.OrderBy(x => x.Key).ToDictionary(k => k.Key, v => v.Value);

            Assert.True(actual.IsEqualOrWrite(expected, _output, true));
        }

    }
}
