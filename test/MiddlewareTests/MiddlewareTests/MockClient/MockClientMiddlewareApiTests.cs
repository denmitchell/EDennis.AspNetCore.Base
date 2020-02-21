using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using Lcr = MockClientApi.Launcher;
using Lib = MockClientApi.Lib;

namespace EDennis.AspNetCore.MiddlewareTests {

    /// <summary>
    /// Note: IClassFixture was not used because
    /// the individual test cases were conflicting with each
    /// (possibly, one test case was updating the configuration
    /// while another test case was calling Get).  To resolve
    /// the issue, I instantiate the LauncherFixture within each
    /// test method.  This is inefficient, but it works.
    /// </summary>
    [Collection("Sequential")]
    public class MockClientApiTests  {

        private readonly ITestOutputHelper _output;

        public MockClientApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        protected class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("MockClientApi", "PersonController",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "MockClient\\TestJson.xlsx") {
            }
        }


        [Theory]
        [TestJsonA("GetA", "Full Application Authorization", "A")]
        [TestJsonA("GetA", "Full Controller Authorization", "B")]
        [TestJsonA("GetA", "GetA Method Authorization", "C")]
        [TestJsonA("GetA", "GetB Method Authorization", "D")]
        [TestJsonA("GetB", "Full Application Authorization", "A")]
        [TestJsonA("GetB", "Full Controller Authorization", "B")]
        [TestJsonA("GetB", "GetA Method Authorization", "C")]
        [TestJsonA("GetB", "GetB Method Authorization", "D")]
        public virtual void Get(string t, JsonTestCase jsonTestCase) {

            using var fixture = new LauncherFixture<Lib.Program, Lcr.Program>();
            var client = fixture.HttpClient;
            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"MockClient\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg).StatusCode;

            var method = jsonTestCase.MethodName;
            var expected = jsonTestCase.GetObject<int>("Expected");

            var url = $"Person/{method}";

            var result = client.GetAsync(client.BaseAddress.ToString() + url).Result;
            var actual = (int)result.StatusCode;


            Assert.Equal((int)System.Net.HttpStatusCode.OK, status);
            Assert.True(actual.IsEqualOrWrite(expected, _output, true));

        }

    }
}
