using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors.InternalApi;
using EDennis.Samples.Colors.InternalApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class MultitierIntegrationTests_InMemory : InMemoryIntegrationTests<EDennis.Samples.Colors.ExternalApi.Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public MultitierIntegrationTests_InMemory(ITestOutputHelper output, InMemoryWebApplicationFactory<EDennis.Samples.Colors.ExternalApi.Startup> factory)
            :base(output,factory){}


        [Theory]
        [InlineData(1, "black")]
        [InlineData(2, "white")]
        [InlineData(3, "gray")]
        [InlineData(4, "red")]
        [InlineData(5, "green")]
        [InlineData(6, "blue")]
        public void Get(int id, string expectedName) {
            _output.WriteLine($"Instance Name:{_instanceName}");

            var color = _client.Get<Color>($"iapi/color/{id}").Value;

            Assert.Equal(expectedName, color.Name);

        }


        [Fact]
        public void Post() {
            _output.WriteLine($"Instance Name:{_instanceName}");

            _client.Post("iapi/color", new Color { Name = "burgundy" });
            var colors = _client.Get<List<Color>>("iapi/color").Value;

            Assert.Equal("burgundy", colors.First(x => x.Id == 7).Name);

            Assert.Collection(colors,
                new Action<Color>[] {
                    item=>Assert.Contains("black",item.Name),
                    item=>Assert.Contains("blue",item.Name),
                    item=>Assert.Contains("burgundy",item.Name),
                    item=>Assert.Contains("gray",item.Name),
                    item=>Assert.Contains("green",item.Name),
                    item=>Assert.Contains("red",item.Name),
                    item=>Assert.Contains("white",item.Name)
                });
        }



    }
}
