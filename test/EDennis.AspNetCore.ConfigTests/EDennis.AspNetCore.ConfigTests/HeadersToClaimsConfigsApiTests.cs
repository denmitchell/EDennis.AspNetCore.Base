using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.HeadersToClaimsConfigsApi.Tests;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    public class HeadersToClaimsConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public HeadersToClaimsConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void TestHeadersToClaims() {

            var client = _factory.CreateClient["HeadersToClaimsConfigsApi"]();
            var result = client.Get<HeadersToClaims>($"HeadersToClaims");
            HeadersToClaims obj = (HeadersToClaims)result.Value;

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);

            Assert.Equal("user_scope", obj.PreAuthentication["X-UserScope"] );
            Assert.Equal("role", obj.PostAuthentication["X-Role"]);
            Assert.Equal("name", obj.PostAuthentication["X-User"]);

        }


    }
}