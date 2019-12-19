using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
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


        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("HelloWorld1", "HelloWorld",
                      methodName, testScenario, testCase, NetCoreTestingUtilities.DatabaseProvider.Excel, "TestJson.xlsx") {
            }
        }
        /*
         
        [Theory]
        [TestJsonA("GetPerson", "", "A")]
        [TestJsonA("GetPerson", "", "B")]
        public void GetPerson(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine($"Test case: {t}");

            var first = jsonTestCase.GetObject<string>("First");
            var last = jsonTestCase.GetObject<string>("Last");
            var expected = jsonTestCase.GetObject<Person>("Expected");

            var hw = new HelloWorld();
            var actual = hw.GetPerson(first, last);


            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.LastName, actual.LastName);

        }
    

    */


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