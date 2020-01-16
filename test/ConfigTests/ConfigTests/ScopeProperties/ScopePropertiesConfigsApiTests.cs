using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.ScopePropertiesConfigsApi.Tests;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    [Collection("Sequential")]
    public class ScopePropertiesConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public ScopePropertiesConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void TestScopeProperties() {

            var client = _factory.CreateClient["ScopePropertiesApi"]();
            var result = client.Get<ScopePropertiesSettings>($"ScopeProperties");
            ScopePropertiesSettings obj = (ScopePropertiesSettings)result.Value;

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);


            Assert.Equal(UserSource.JwtSubjectClaim, obj.UserSources.AuthenticatedUserSource);
            Assert.Equal(UserSource.XUserHeader, obj.UserSources.UnauthenticatedUserSource);

            Assert.True(obj.AppendHostPath);
            Assert.True(obj.CopyClaims);
            Assert.True(obj.CopyHeaders);

        }


    }
}