using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.ApiConfigsApi.Apis;
using EDennis.Samples.ApiConfigsApi.Tests;
using System;
using System.Collections.Generic;
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
            var result = client.Get<string>($"Api{apiSuffix}");
            Assert.Equal($"Api{apiSuffix}", result.Value);

        }


    }
}