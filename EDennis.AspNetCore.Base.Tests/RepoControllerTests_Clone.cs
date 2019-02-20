using EDennis.Samples.Colors.InternalApi;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;
using EDennis.Samples.Colors.InternalApi.Models;
using System.Collections.Generic;
using System.Linq;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;

namespace EDennis.AspNetCore.Base.Testing {
    public class RepoControllerTests_Clone : CloneIntegrationTests<Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };


        public RepoControllerTests_Clone(ITestOutputHelper output, CloneWebApplicationFactory<Startup> factory)
            : base(output,factory){}


        /// <summary>
        /// Optional internal class ... reduced the number of parameters in TestJson attribute
        /// by specifying constant parameter values for className and testJsonConfigPath here
        /// </summary>
        internal class TestJsonSpecific : TestJsonAttribute {
            public TestJsonSpecific(string methodName, string testScenario, string testCase)
                : base("ColorRepo", methodName, testScenario, testCase, "TestJsonConfigs\\InternalApi.json") {
            }
        }



        [Theory]
        [TestJsonSpecific("GetById", "SqlRepo", "1")]
        [TestJsonSpecific("GetById", "SqlRepo", "2")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);
            _output.WriteLine($"Instance Name:{_cloneIndex}");

            var id = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = _client.Get<Color>($"iapi/color/{id}").Value;

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }



        [Theory]
        [TestJsonSpecific("Post", "SqlRepo", "brown")]
        [TestJsonSpecific("Post", "SqlRepo", "orange")]
        public void Post(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            _client.Post("iapi/color", input);
            var actual = _client.Get<List<Color>>("iapi/color")
                    .Value
                    .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJsonSpecific("Update", "SqlRepo", "1")]
        [TestJsonSpecific("Update", "SqlRepo", "2")]
        public void Put(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var id = input.Id;
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            _client.Put($"iapi/color/{id}", input);
            var actual = _client.Get<List<Color>>("iapi/color")
                .Value
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }



        [Theory]
        [TestJsonSpecific("Delete", "SqlRepo", "3")]
        [TestJsonSpecific("Delete", "SqlRepo", "4")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            _client.Delete<Color>($"iapi/color/{input}");
            var actual = _client.Get<List<Color>>("iapi/color")
                .Value
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }


        [Theory]
        [InlineData(1, "black")]
        [InlineData(2, "white")]
        [InlineData(3, "gray")]
        [InlineData(4, "red")]
        [InlineData(5, "green")]
        [InlineData(6, "blue")]
        public void Get_Inline(int id, string expectedName) {
            _output.WriteLine($"Instance Name:{_cloneIndex}");

            var color = _client.Get<Color>($"iapi/color/{id}").Value;

            Assert.Equal(expectedName, color.Name);

        }


        [Fact]
        public void Post_Fact() {
            _output.WriteLine($"Instance Name:{_cloneIndex}");

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

        [Fact]
        public void Put_Fact() {
            _output.WriteLine($"Instance Name:{_cloneIndex}");

            _client.Put("iapi/color/1", new Color { Id = 1, Name = "burgundy" });
            var colors = _client.Get<List<Color>>("iapi/color").Value;

            Assert.Equal("burgundy", colors.First(x => x.Id == 1).Name);

            Assert.Collection(colors,
                new Action<Color>[] {
                    item=>Assert.Contains("blue",item.Name),
                    item=>Assert.Contains("burgundy",item.Name),
                    item=>Assert.Contains("gray",item.Name),
                    item=>Assert.Contains("green",item.Name),
                    item=>Assert.Contains("red",item.Name),
                    item=>Assert.Contains("white",item.Name)
                });
        }


        [Fact]
        public void Delete_Fact() {
            _output.WriteLine($"Instance Name:{_cloneIndex}");

            _client.Delete("iapi/color/3", new Color { Id = 3, Name = "gray" });
            var colors = _client.Get<List<Color>>("iapi/color").Value;

            Assert.Collection(colors,
                new Action<Color>[] {
                    item=>Assert.Contains("black",item.Name),
                    item=>Assert.Contains("blue",item.Name),
                    item=>Assert.Contains("green",item.Name),
                    item=>Assert.Contains("red",item.Name),
                    item=>Assert.Contains("white",item.Name)
                });
        }



    }
}
