using Divergic.Logging.Xunit;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Colors.InternalApi;
using EDennis.Samples.Colors.InternalApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class RepoControllerTests : WriteableTemporalIntegrationTests<Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };
        private readonly ICacheLogger _logger;

        public RepoControllerTests(ITestOutputHelper output, ConfiguringWebApplicationFactory<Startup> factory)
            :base(output,factory){
            _logger = Output.BuildLogger();
        }


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("ColorDb", "EDennis.Samples.Colors.InternalApi", "ColorRepo",  methodName, testScenario, testCase) {
            }
        }



        [Theory]
        [TestJson_("GetById", "SqlRepo", "1")]
        [TestJson_("GetById", "SqlRepo", "2")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Instance Name:{InstanceName}");

            var id = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = HttpClient.Get<Color>($"iapi/color/{id}").Value;

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }



        [Theory]
        [TestJson_("Create", "SqlRepo", "brown")]
        [TestJson_("Create", "SqlRepo", "orange")]
        public void Post(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            HttpClient.Post("iapi/color", input);
            var actual = HttpClient.Get<List<Color>>("iapi/color")
                    .Value
                    .OrderBy(x=>x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Update", "SqlRepo", "1")]
        [TestJson_("Update", "SqlRepo", "2")]
        public void Put(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var id = input.Id;
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            HttpClient.Put($"iapi/color/{id}", input);
            var actual = HttpClient.Get<List<Color>>("iapi/color")
                .Value
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }



        [Theory]
        [TestJson_("Delete", "SqlRepo", "3")]
        [TestJson_("Delete", "SqlRepo", "4")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            HttpClient.Delete<Color>($"iapi/color/{input}");
            var actual = HttpClient.Get<List<Color>>("iapi/color")
                .Value
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }


        [Theory]
        [InlineData(1, "black")]
        [InlineData(2, "white")]
        [InlineData(3, "gray")]
        [InlineData(4, "red")]
        [InlineData(5, "green")]
        [InlineData(6, "blue")]
        public void Get_Inline(int id, string expectedName) {
            Output.WriteLine($"Instance Name:{InstanceName}");

            var color = HttpClient.Get<Color>($"iapi/color/{id}").Value;

            Assert.Equal(expectedName, color.Name);

        }


        [Fact]
        public void Post_Fact() {
            Output.WriteLine($"Instance Name:{InstanceName}");

            HttpClient.Post("iapi/color", new Color { Name = "burgundy" });
            var colors = HttpClient.Get<List<Color>>("iapi/color").Value;

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
            Output.WriteLine($"Instance Name:{InstanceName}");

            HttpClient.Put("iapi/color/1", new Color { Id = 1, Name = "burgundy" });
            var colors = HttpClient.Get<List<Color>>("iapi/color").Value
                .OrderBy(x=>x.Name);

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
            Output.WriteLine($"Instance Name:{InstanceName}");

            HttpClient.Delete("iapi/color/3", new Color { Id = 3, Name = "gray" });
            var colors = HttpClient.Get<List<Color>>("iapi/color").Value;

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
