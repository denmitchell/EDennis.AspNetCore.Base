﻿using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.IO;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;
using Lib = ScopedLoggerApi.Lib;
using Lcr = ScopedLoggerApi.Launcher;
using EDennis.AspNetCore.Base.Testing;

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
    public class ScopedTraceLoggerApiTests {

        private readonly ITestOutputHelper _output;

        public ScopedTraceLoggerApiTests(
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

            using var fixture = new LauncherFixture<Lib.Program, Lcr.Program>();
            var client = fixture.HttpClient;

            client.DefaultRequestHeaders.Add(Constants.ENTRYPOINT_KEY, EntryPoint.TestProject.ToString());

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
