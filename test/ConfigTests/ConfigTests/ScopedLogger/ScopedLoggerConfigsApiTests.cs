using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.ScopedLoggerApi.Tests;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    [Collection("Sequential")]
    public class ScopedLoggerConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public ScopedLoggerConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void TestScopedLogger() {

            var client = _factory.CreateClient["ScopedLoggerApi"]();
            var result = client.Get<ScopedLoggerSettings>($"ScopedLogger");
            ScopedLoggerSettings obj = (ScopedLoggerSettings)result.Value;

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);

            Assert.Equal(AssignmentKeySource.User, obj.AssignmentKeySource);
            Assert.Null(obj.AssignmentKeyName);

        }


    }
}