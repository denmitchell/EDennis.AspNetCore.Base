using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.ApiConfigsApi.Apis;
using EDennis.Samples.ApiConfigsApi.Tests;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    public class ApiConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public ApiConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void TestApis(int apiSuffix) {

            var client = _factory.CreateClient["ApiConfigsApi"]();
            var result = client.Get<Dictionary<string,string>>($"Api{apiSuffix}");
            Dictionary<string,string> obj = (Dictionary<string, string>)result.Value;



            Assert.Equal($"Api{apiSuffix}", obj["ApiName"]);
            Assert.Contains($"http://localhost", obj["HttpClientBaseAddress"]);
            Assert.Equal("4", obj["ApisCount"]);
            Assert.Equal("ApiConfigsApi", obj["SecureTokenServiceApplicationName"]);
            Assert.Equal("Information", obj["LoggerLevel"]);
            Assert.Equal("None", obj["ScopedLoggerLevel"]);

        }
        /*
                        HttpClientBaseAddress = HttpClient.BaseAddress.ToString(),
                        Apis = apis.CurrentValue,
                        ScopePropertiesUser = ScopeProperties.User,
                        SecureTokenServiceApplicationName = secureTokenService.ApplicationName,
                        LoggerLevel = Logger.EnabledAt(),
                        ScopedLoggerLevel = scopedLogger.LogLevel

        */
    }
}