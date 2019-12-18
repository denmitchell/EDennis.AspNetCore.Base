using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.ScopePropertiesMiddlewareApi.Tests;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.MiddlewareTests {
    [Collection("Sequential")]
    public class ScopePropertiesMiddlewareApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;
        private HttpClient _client;

        private readonly ITestOutputHelper _output;
        public ScopePropertiesMiddlewareApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;

            _client = _factory.CreateClient["ScopePropertiesMiddlewareApi"]();

        }


        //[Theory]
        //[InlineData(0)]
        //[InlineData(1)]
        //public void TestScopeProperties(int testCase) {

        //    var result = _client.Get<ScopeProperties>($"ScopeProperties");
        //    ScopeProperties obj = (ScopeProperties)result.Value;

        //    var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
        //    _output.WriteLine(json);


        //    Assert.Equal(UserSource.JwtSubjectClaim, obj.UserSource);
        //    Assert.True(obj.AppendHostPath);
        //    Assert.True(obj.CopyClaims);
        //    Assert.True(obj.CopyHeaders);

        //}


    }
}