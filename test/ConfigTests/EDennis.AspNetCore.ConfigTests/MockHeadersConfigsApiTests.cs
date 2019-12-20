using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.MockHeadersConfigsApi.Tests;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    [Collection("Sequential")]
    public class MockHeadersConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public MockHeadersConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void TestMockHeaders() {

            var client = _factory.CreateClient["MockHeadersConfigsApi"]();
            var result = client.Get<MockHeaderSettingsCollection>($"MockHeaders");
            MockHeaderSettingsCollection obj = (MockHeaderSettingsCollection)result.Value;

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);

            Assert.Collection(obj["X-User"].Values,
                 item => Assert.Contains("moe@stooges.org", item));
            Assert.Equal(MockHeaderConflictResolution.Overwrite, obj["X-User"].ConflictResolution);
            Assert.Collection(obj["X-Role"].Values,
                 item => Assert.Contains("admin", item),
                 item => Assert.Contains("user", item)
                 );
            Assert.Equal(MockHeaderConflictResolution.Union, obj["X-Role"].ConflictResolution);

        }


    }
}