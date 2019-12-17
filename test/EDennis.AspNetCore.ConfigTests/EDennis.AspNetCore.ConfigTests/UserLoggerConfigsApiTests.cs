using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.UserLoggerConfigsApi.Tests;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    [Collection("Sequential")]
    public class UserLoggerConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public UserLoggerConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void TestUserLogger() {

            var client = _factory.CreateClient["UserLoggerConfigsApi"]();
            var result = client.Get<UserLoggerSettings>($"UserLogger");
            UserLoggerSettings obj = (UserLoggerSettings)result.Value;

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);

            Assert.Equal(UserSource.JwtSubjectClaim, obj.UserSource);

        }


    }
}