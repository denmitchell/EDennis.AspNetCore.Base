using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.MockClientMiddlewareApi.Tests;
using System.IO;
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
    public class MockClientMiddlewareApiTests {

        private readonly ITestOutputHelper _output;

        public MockClientMiddlewareApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("MockClientApi", "PersonController",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "MockClient\\TestJson.xlsx") {
            }
        }


        [Theory]
        [TestJsonA("GetA", "", "Full Application Authorization")]
        [TestJsonA("GetA", "", "Full Controller Authorization")]
        [TestJsonA("GetA", "", "GetA Method Authorization")]
        [TestJsonA("GetA", "", "GetB Method Authorization")]
        [TestJsonA("GetB", "", "Full Application Authorization")]
        [TestJsonA("GetB", "", "Full Controller Authorization")]
        [TestJsonA("GetB", "", "GetA Method Authorization")]
        [TestJsonA("GetB", "", "GetB Method Authorization")]
        public void Get(string t, JsonTestCase jsonTestCase) {

            using var factory = new TestApis();
            var client = factory.CreateClient["MockClientApi"]();
            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"MockClient\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var method = jsonTestCase.MethodName;
            var expected = jsonTestCase.GetObject<int>("Expected");

            var url = $"Person/{method}";

            var result = client.GetAsync(client.BaseAddress.ToString() + url).Result;
            var actual = (int)result.StatusCode;

            Assert.True(actual.IsEqualOrWrite(expected, _output, true));
        }

    }
}
