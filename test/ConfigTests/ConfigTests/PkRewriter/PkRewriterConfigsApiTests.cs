using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using PkRewriterApi.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    [Collection("Sequential")]
    public class PkRewriterConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public PkRewriterConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void TestPkRewriter() {

            var client = _factory.CreateClient["PkRewriterApi"]();
            var result = client.Get<PkRewriterSettings>($"PkRewriter");
            PkRewriterSettings obj = (PkRewriterSettings)result.Value;

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);

            Assert.True(obj.Enabled);
            Assert.Equal(-999, obj.BasePrefix);
            Assert.Equal("DeveloperName", obj.DeveloperNameEnvironmentVariable);
            Assert.Equal(
                new Dictionary<string, int> {
                        { "moe",-501 },
                        { "larry",-502 },
                        { "curly",-503 },
                    }
                .OrderBy(x => x.Key), obj.DeveloperPrefixes.OrderBy(x => x.Key));
        }


    }
}