using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.ScopePropertiesMiddlewareApi.Tests;
using System;
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

            _client = _factory.CreateClient["ScopePropertiesMiddlewareApi"]();

        }


        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("HelloWorld1", "HelloWorld",
                      methodName, testScenario, testCase, NetCoreTestingUtilities.DatabaseProvider.Excel, "TestJson.xlsx") {
            }
        }
        
         
        [Theory]
        [TestJsonA("Get", "", "A")]
        [TestJsonA("Get", "", "B")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine($"Test case: {t}");

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "A");

            IScopeProperties actual = null;
            IScopeProperties expected = null;

            try {
                var claimsQueryString = jsonTestCase.GetObject<string>("Claims");
                var headersQueryString = jsonTestCase.GetObject<string>("Headers");
                expected = jsonTestCase.GetObject<IScopeProperties>("Expected");

                var url = $"ScopeProperties?{claimsQueryString}&{headersQueryString}";

                var result = _client.Get<ScopeProperties>(url);
                actual = (ScopeProperties)result.Value;

                var json = JsonSerializer.Serialize(actual, new JsonSerializerOptions { WriteIndented = true });
                _output.WriteLine(json);
            } finally {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", env);
            }

            Assert.True(actual.IsEqualOrWrite(expected,_output,true));

        }

    }
}
