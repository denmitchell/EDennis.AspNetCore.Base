using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.MockHeadersMiddlewareApi.Tests;
using PkRewriterApi;
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
    public class PkRewriterMiddlewareApiTests {

        private readonly ITestOutputHelper _output;

        public PkRewriterMiddlewareApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("PkRewriterApi", "PersonController",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "PkRewriter\\TestJson.xlsx") {
            }
        }


        [Theory]
        [TestJsonA("Create", "", "A")]
        [TestJsonA("Create", "", "B")]
        [TestJsonA("Create", "", "C")]
        public void Create(string t, JsonTestCase jsonTestCase) {

            using var factory = new TestApis();
            var client = factory.CreateClient["PkRewriterApi"]();
            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"PkRewriter\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var url = $"Person";

            var input1 = jsonTestCase.GetObject<Person>("Input1");
            var expected1 = jsonTestCase.GetObject<List<Person>>("Expected1");

            client.Post(url, input1);
            var result1 = client.Get<List<Person>>(url);
            var actual1 = result1.GetObject<List<Person>>();
            Assert.True(actual1.IsEqualOrWrite(expected1, _output, true));


            var expected1b = jsonTestCase.GetObject<List<Person>>("Expected1b");
            var result1b = client.Get<List<Person>>($"{url}?{Constants.PK_REWRITER_BYPASS_KEY}=true");
            var actual1b = result1b.GetObject<List<Person>>();
            Assert.True(actual1b.IsEqualOrWrite(expected1b, _output, true));

        }



        [Theory]
        [TestJsonA("Update", "", "A")]
        [TestJsonA("Update", "", "B")]
        [TestJsonA("Update", "", "C")]
        public void Update(string t, JsonTestCase jsonTestCase) {

            using var factory = new TestApis();
            var client = factory.CreateClient["PkRewriterApi"]();
            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"PkRewriter\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var url = $"Person";

            var input2 = jsonTestCase.GetObject<Person>("Input2");
            var id = jsonTestCase.GetObject<int>("Id2");
            var expected2 = jsonTestCase.GetObject<List<Person>>("Expected2");

            client.Put($"{url}/{id}", input2);
            var result2 = client.Get<List<Person>>(url);
            var actual2 = result2.GetObject<List<Person>>();
            Assert.True(actual2.IsEqualOrWrite(expected2, _output, true));

            var result2b = client.Get<List<Person>>($"{url}?{Constants.PK_REWRITER_BYPASS_KEY}=true");
            var actual2b = result2.GetObject<List<Person>>();
            var expected2b = jsonTestCase.GetObject<List<Person>>("Expected2b");
            Assert.True(actual2b.IsEqualOrWrite(expected2b, _output, true));

        }



        [Theory]
        [TestJsonA("Query", "", "A")]
        [TestJsonA("Query", "", "B")]
        [TestJsonA("Query", "", "C")]
        public void Query(string t, JsonTestCase jsonTestCase) {

            using var factory = new TestApis();
            var client = factory.CreateClient["PkRewriterApi"]();
            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"PkRewriter\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var url = $"Person";

            var queryString3 = jsonTestCase.GetObject<string>("QueryString3");
            var expected3 = jsonTestCase.GetObject<List<Person>>("Expected3");

            client.Get<List<Person>>($"{url}?{queryString3}");
            var result3 = client.Get<List<Person>>(url);
            var actual3 = result3.GetObject<List<Person>>();
            Assert.True(actual3.IsEqualOrWrite(expected3, _output, true));


        }



    }
}
