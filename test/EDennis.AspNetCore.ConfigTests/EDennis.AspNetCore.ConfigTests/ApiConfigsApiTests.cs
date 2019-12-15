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


        [Fact]
        public void TestApi1() {

            var client = _factory.CreateClient["ApiConfigsApi"]();
            var result = client.Get<Api1>("Api1");
            var api1 = (Api1)result.Value;
            if (api1 == null)
                throw new ApplicationException($"Cannot retrieve api1 from endpoint: {client.BaseAddress}Api");

        }


    }
}