using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.MockClientConfigsApi.Tests;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    [Collection("Sequential")]

    public class MockClientConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public MockClientConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void TestMockClient() {

            var client = _factory.CreateClient["MockClientConfigsApi"]();
            var result = client.Get<ActiveMockClientSettings>($"MockClient");
            ActiveMockClientSettings obj = (ActiveMockClientSettings)result.Value;

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);

            Assert.Equal("MockClient2", obj.ActiveMockClientKey );
            Assert.Equal("EDennis.Samples.SomeApi2", obj.ActiveMockClient.ClientId);
            Assert.Equal("some secret 2", obj.ActiveMockClient.ClientSecret);
            Assert.Collection(obj.ActiveMockClient.Scopes, 
                 item => Assert.Contains("some scope 2a", item),
                 item => Assert.Contains("some scope 2b", item));

        }


    }
}