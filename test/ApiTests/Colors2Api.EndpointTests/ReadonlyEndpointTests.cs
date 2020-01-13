using Colors2.Models;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using L = Colors2Api.Lib;


namespace Colors2Api.EndpointTests {

    [Collection("Endpoint Tests")]
    public class ReadonlyEndpointTests
        : SqlServerReadonlyEndpointTests<Hsl, L.Program, Colors2ApiLauncher> {

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
        [TestJson_("GetDynamicLinq", "ReadonlyEndpointTests|WhereSkipTake", "A")]
        [TestJson_("GetDynamicLinq", "ReadonlyEndpointTests|WhereOrderBySelectTake", "B")]
        public void GetDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "ReadonlyEndpointTests|WhereSkipTake", "A")]
        [TestJson_("GetDynamicLinq", "ReadonlyEndpointTests|WhereOrderBySelectTake", "B")]
        public async Task GetDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetFromStoredProcedure", "ReadonlyEndpointTests|HslByColorName", "A")]
        [TestJson_("GetFromStoredProcedure", "ReadonlyEndpointTests|HslByColorName", "B")]
        public void GetFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetFromStoredProcedure", "ReadonlyEndpointTests|HslByColorName", "A")]
        [TestJson_("GetFromStoredProcedure", "ReadonlyEndpointTests|HslByColorName", "B")]
        public async Task GetFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetJsonColumnFromStoredProcedure", "ReadonlyEndpointTests|RgbJsonByColorName", "A")]
        [TestJson_("GetJsonColumnFromStoredProcedure", "ReadonlyEndpointTests|RgbJsonByColorName", "B")]
        public void GetJsonColumnFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetJsonColumnFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetJsonColumnFromStoredProcedure", "ReadonlyEndpointTests|RgbJsonByColorName", "A")]
        [TestJson_("GetJsonColumnFromStoredProcedure", "ReadonlyEndpointTests|RgbJsonByColorName", "B")]
        public async Task GetJsonColumnFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetJsonColumnFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

    }
}
