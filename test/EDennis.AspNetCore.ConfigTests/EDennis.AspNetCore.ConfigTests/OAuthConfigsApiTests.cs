using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.OAuthConfigsApi.Tests;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    public class OAuthConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public OAuthConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void TestOAuth() {

            var client = _factory.CreateClient["OAuthConfigsApi"]();
            var _ = client.Get<string>($"OAuth/NonSecure");

            //visually inspect OAuthController constructor
            //  for private variables in jwtBearerOptions

        }


    }
}