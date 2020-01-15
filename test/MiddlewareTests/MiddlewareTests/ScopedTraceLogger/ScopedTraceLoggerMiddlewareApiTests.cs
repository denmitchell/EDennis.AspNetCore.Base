using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.ScopedLoggerMiddlewareApi.Tests;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

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
    public class ScopedTraceLoggerMiddlewareApiTests {

        private readonly ITestOutputHelper _output;

        public ScopedTraceLoggerMiddlewareApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("ScopedLoggerApi", "ScopedLoggerController",
                      methodName, testScenario, testCase, NetCoreTestingUtilities.DatabaseProvider.Excel, "ScopedTraceLogger\\TestJson.xlsx") {
            }
        }


        [Theory]
        [TestJsonA("Get", "", "A")]
        [TestJsonA("Get", "", "B")]
        [TestJsonA("Get", "", "C")]
        public void Get(string t, JsonTestCase jsonTestCase) {

            using var factory = new TestApis();
            var client = factory.CreateClient["ScopedLoggerApi"]();
            _output.WriteLine($"Test case: {t}");

            TestUrl(jsonTestCase, client, 1);
            TestUrl(jsonTestCase, client, 2);
            TestUrl(jsonTestCase, client, 3);

        }


        private void TestUrl(JsonTestCase jsonTestCase, HttpClient client, int index) {

            //send configuration for test case
            var jcfg = File.ReadAllText($"ScopedTraceLogger\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var qry = jsonTestCase.GetObject<string>($"QueryString{index}");
            var expected = jsonTestCase.GetObject<string>($"Expected{index}");

            var url = $"ScopedLogger?{qry}";

            var result = client.GetAsync(client.BaseAddress.ToString() + url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            _output.WriteLine($"{jsonTestCase.TestCase}({index}): {content}");

            var actual = content;

            Assert.True(actual.IsEqualOrWrite(expected, _output, true));

        }

    }
}
