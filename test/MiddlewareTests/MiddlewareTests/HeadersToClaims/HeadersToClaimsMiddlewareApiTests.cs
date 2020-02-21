using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using EDennis.AspNetCore.Base.Testing;
using Lib = HeadersToClaimsApi.Lib;
using Lcr = HeadersToClaimsApi.Launcher;
using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Threading;
using HeadersToClaimsApi.Tests;

namespace MiddlewareTests {


    public class HeadersToClaimsApiTests : IDisposable {

        private readonly ITestOutputHelper _output;

        LauncherFixture<Lib.Program, Lcr.Program> fixture;
        HttpClient client;

        public HeadersToClaimsApiTests(
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
        public virtual void Get(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine($"Test case: {t}");

            var factory = new TestApis();
            var client = factory.CreateClient["HeadersToClaimsApi"]();

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

        public void Dispose() {
            Sequentializer.Gatekeeper.Add(true);
        }
    }
}
