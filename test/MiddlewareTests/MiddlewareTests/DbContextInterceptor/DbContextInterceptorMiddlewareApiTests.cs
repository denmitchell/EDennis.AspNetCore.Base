using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.DbContextInterceptorMiddlewareApi.Lib;
using EDennis.Samples.DbContextInterceptorMiddlewareApi.Tests;
using System.Collections.Generic;
using System.IO;
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
    public class DbContextInterceptorMiddlewareApiTests {

        private readonly ITestOutputHelper _output;

        public DbContextInterceptorMiddlewareApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        internal class TestJsonPerson : TestJsonAttribute {
            public TestJsonPerson(string methodName, string testScenario, string testCase)
                : base("DbContextInterceptorApi", "PersonController",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "DbContextInterceptor\\TestJson.xlsx") {
            }
        }

        internal class TestJsonPosition : TestJsonAttribute {
            public TestJsonPosition(string methodName, string testScenario, string testCase)
                : base("DbContextInterceptorApi", "PersonController",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "DbContextInterceptor\\TestJson.xlsx") {
            }
        }

        [Theory]
        [TestJsonPerson("CUD", "", "A")]
        [TestJsonPerson("CUD", "", "B")]
        [TestJsonPerson("CUD", "", "C")]
        public void Person(string t, JsonTestCase jsonTestCase) {

            //TODO: Modify Startup to include PkRewriter
            //TODO: Create test cases in Excel
            //TODO: initiate Create, Update, Delete

            using var factory = new TestApis();
            var client = factory.CreateClient["DbContextInterceptorApi"]();
            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"Person\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var headersQueryString = jsonTestCase.GetObject<string>("Headers");
            //var expected = jsonTestCase.GetObject<List<SimpleClaim>>("Expected");

            var url = $"Person?{headersQueryString}";

            var result = client.GetAsync(client.BaseAddress.ToString() + url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            _output.WriteLine(content);

            //var actual = JsonSerializer.Deserialize<List<SimpleClaim>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //Assert.True(actual.IsEqualOrWrite(expected, _output, true));
        }

        [Theory]
        [TestJsonPosition("CUD", "", "D")]
        [TestJsonPosition("CUD", "", "E")]
        [TestJsonPosition("CUD", "", "F")]
        public void Position(string t, JsonTestCase jsonTestCase) {

            using var factory = new TestApis();
            var client = factory.CreateClient["DbContextInterceptorApi"]();
            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"Person\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var headersQueryString = jsonTestCase.GetObject<string>("Headers");
            //var expected = jsonTestCase.GetObject<List<SimpleClaim>>("Expected");

            var url = $"Person?{headersQueryString}";

            var result = client.GetAsync(client.BaseAddress.ToString() + url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            _output.WriteLine(content);

            //var actual = JsonSerializer.Deserialize<List<SimpleClaim>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //Assert.True(actual.IsEqualOrWrite(expected, _output, true));
        }



    }
}
