using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Colors.ExternalApi;
using EDennis.Samples.Colors.InternalApi;
using EDennis.Samples.Colors.ExternalApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using EDennis.AspNetCore.Base.Testing;

namespace EDennis.AspNetCore.Base.Tests {
    public class ApiClientTests : 
        WriteableApiClientTests<InternalApi,EDennis.Samples.Colors.InternalApi.Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public ApiClientTests(ITestOutputHelper output, 
            ApiLauncherFactory<EDennis.Samples.Colors.InternalApi.Startup> fixture)
            :base(output,fixture){}


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase) 
                : base("ColorDb","EDennis.Samples.Colors.InternalApi","ColorController", methodName, testScenario, testCase) {
            }
        }



        [Theory]
        [TestJson_("Get", "HttpClientExtensions", "1")]
        [TestJson_("Get", "HttpClientExtensions", "2")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine($"Instance Name:{InstanceName}");
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Color>("Expected");
            
            var actual = ApiClient.GetColor(id).Value;

            Assert.True(actual.IsEqualOrWrite(expected,PROPS_FILTER,Output));
        }


        [Theory]
        [TestJson_("Post", "HttpClientExtensions", "brown")]
        [TestJson_("Post", "HttpClientExtensions", "orange")]
        public void Post(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine($"Instance Name:{InstanceName}");
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x=>x.Id);

            ApiClient.Create(input);
            var actual = ApiClient
                .GetColors()
                .Value
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }



        [Theory]
        [InlineData(1, "black")]
        [InlineData(2, "white")]
        [InlineData(3, "gray")]
        [InlineData(4, "red")]
        [InlineData(5, "green")]
        [InlineData(6, "blue")]
        public void Get_InlineData(int id, string expectedName) {
            Output.WriteLine($"Instance Name:{InstanceName}");

            var color = ApiClient
                .GetColor(id)
                .Value;

            Assert.Equal(expectedName, color.Name);

        }


        [Fact]
        public void Post_Fact() {
            Output.WriteLine($"Instance Name:{InstanceName}");

            ApiClient.Create(new Color { Name = "burgundy" });
            var colors = ApiClient
                .GetColors()
                .Value;

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
