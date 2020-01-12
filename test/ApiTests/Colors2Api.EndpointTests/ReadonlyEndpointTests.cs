using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using L=Colors2Api.Lib;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Threading.Tasks;


namespace Colors2Api.EndpointTests {

    [Collection("Endpoint Tests")]
    public class ReadonlyEndpointTests
        : SqlServerReadonlyEndpointTests<L.Program, Colors2ApiLauncher> {

        public ReadonlyEndpointTests(ITestOutputHelper output, 
            LauncherFixture<L.Program, Colors2ApiLauncher> launcherFixture) 
            : base(output, launcherFixture) {
        }


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Api", "HslController", methodName, testScenario, testCase) {
            }
        }



        [Theory]
        [TestJson_("GetOData", "ReadonlyEndpointTests|FilterSkipTop", "A")]
        [TestJson_("GetOData", "ReadonlyEndpointTests|FilterOrderBySelectTop", "B")]
        public void GetOData(string t, JsonTestCase jsonTestCase) {
            var ea = GetOData_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }



        [Theory]
        [TestJson_("GetDevExtreme", "ReadonlyEndpointTests|FilterSkipTake", "A")]
        [TestJson_("GetDevExtreme", "ReadonlyEndpointTests|FilterSortSelectTake", "B")]
        public void GetDevExtreme(string t, JsonTestCase jsonTestCase) {
            var ea = GetDevExtreme_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "WhereSkipTake", "A")]
        [TestJson_("GetDynamicLinq", "WhereOrderBySelectTake", "B")]
        public void GetDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "WhereSkipTake", "A")]
        [TestJson_("GetDynamicLinq", "WhereOrderBySelectTake", "B")]
        public async Task GetDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetFromStoredProcedure", "HslByColorName", "AliceBlue")]
        [TestJson_("GetFromStoredProcedure", "HslByColorName", "DarkKhaki")]
        public void GetFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");

            dynamic expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
                .FirstOrDefault();

            dynamic actual = HttpClient.Get<List<dynamic>>($"api/hsl/sp?spName={spName}&colorName={colorName}")
                .GetObject<List<dynamic>>()
                .FirstOrDefault();

            Assert.Equal(expected, actual);
        }


        //        [Theory]
        //        [TestJson_("GetFromStoredProcedureAsync", "HslByColorName", "AliceBlue")]
        //        [TestJson_("GetFromStoredProcedureAsync", "HslByColorName", "DarkKhaki")]
        //        public void GetFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
        //            Output.WriteLine(t);
        //            Output.WriteLine($"Db instance name: {InstanceName}");

        //            var spName = jsonTestCase.GetObject<string>("SpName");
        //            var colorName = jsonTestCase.GetObject<string>("ColorName");

        //            dynamic expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //                .FirstOrDefault();

        //            dynamic actual = HttpClient.Get<List<dynamic>>($"api/hsl/sp/async?spName={spName}&colorName={colorName}")
        //                .GetObject<List<dynamic>>()
        //                .FirstOrDefault();

        //            Assert.Equal(expected, actual);
        //        }

        //        [Theory]
        //        [TestJson_("GetJsonColumnFromStoredProcedure", "RgbJsonByColorName", "AliceBlue")]
        //        [TestJson_("GetJsonColumnFromStoredProcedure", "RgbJsonByColorName", "DarkKhaki")]
        //        public void GetJsonColumnFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
        //            Output.WriteLine(t);
        //            Output.WriteLine($"Db instance name: {InstanceName}");

        //            var spName = jsonTestCase.GetObject<string>("SpName");
        //            var colorName = jsonTestCase.GetObject<string>("ColorName");

        //            dynamic expected =
        //                JToken.Parse(
        //                 JToken.Parse(jsonTestCase.GetObject<string>("Expected"))
        //                    .SelectToken("Json")
        //                    .ToString())
        //                .ToObject<dynamic>();


        //            var expectedRgb = new {
        //                expected.Red,
        //                expected.Green,
        //                expected.Blue
        //            };

        //            dynamic actual = JToken.Parse(HttpClient.Get<string>($"api/hsl/json?spName={spName}&colorName={colorName}")
        //                .GetObject<string>())
        //                .ToObject<dynamic>();

        //            var actualRgb = new {
        //                actual.Red,
        //                actual.Green,
        //                actual.Blue
        //            };

        //            Assert.Equal(expectedRgb,actualRgb);
        //        }
    }
}
