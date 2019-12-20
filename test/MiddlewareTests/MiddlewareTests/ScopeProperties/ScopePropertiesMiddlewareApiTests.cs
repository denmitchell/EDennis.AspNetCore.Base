using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.ScopePropertiesMiddlewareApi.Tests;
using IdentityServer4.Endpoints.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.MiddlewareTests {
    [Collection("Sequential")]
    public class ScopePropertiesMiddlewareApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;
        private readonly HttpClient _client;

        private readonly ITestOutputHelper _output;

        public ScopePropertiesMiddlewareApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;

            _client = _factory.CreateClient["ScopePropertiesApi"]();
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
            _output.WriteLine($"Test case: {t}");

            //send configuration for test case
            var jcfg = File.ReadAllText($"ScopeProperties\\{jsonTestCase.TestCase}.json");
            var status = _client.Configure("",jcfg);

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var claimsQueryString = jsonTestCase.GetObject<string>("Claims");
            var headersQueryString = jsonTestCase.GetObject<string>("Headers");
            var expected = jsonTestCase.GetObject<ScopeProperties>("Expected");

            var url = $"ScopeProperties?{claimsQueryString}&{headersQueryString}";

            var result = _client.Get<Dictionary<string,string>>(url);
            var actual = (Dictionary<string, string>)result.Value;

            var json = JsonSerializer.Serialize(actual, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);

            Assert.True(actual.IsEqualOrWrite(expected,_output,true));

        }

    }
}
