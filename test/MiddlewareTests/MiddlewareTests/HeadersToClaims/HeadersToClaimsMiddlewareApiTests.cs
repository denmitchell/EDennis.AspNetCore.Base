using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using EDennis.AspNetCore.Base.Testing;
using Lib = EDennis.Samples.HeadersToClaimsMiddlewareApi.Lib;
using Lcr = EDennis.Samples.HeadersToClaimsMiddlewareApi.Launcher;

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
    public class HeadersToClaimsMiddlewareApiTests {

        private readonly ITestOutputHelper _output;

        public HeadersToClaimsMiddlewareApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("HeadersToClaimsApi", "ClaimsController",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "HeadersToClaims\\TestJson.xlsx") {
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
            var jcfg = File.ReadAllText($"HeadersToClaims\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var headersQueryString = jsonTestCase.GetObject<string>("Headers");
            var expected = jsonTestCase.GetObject<List<Lib.SimpleClaim>>("Expected");

            var url = $"Claims?{headersQueryString}";

            var result = client.GetAsync(client.BaseAddress.ToString() + url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            _output.WriteLine(content);

            var actual = JsonSerializer.Deserialize<List<Lib.SimpleClaim>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(actual.IsEqualOrWrite(expected, _output, true));
        }

    }
}
