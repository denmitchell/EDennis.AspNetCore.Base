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

            if (HttpClient.DefaultRequestHeaders.Contains("X-User"))
                HttpClient.DefaultRequestHeaders.Remove("X-User");
            HttpClient.DefaultRequestHeaders.Add("X-User", "tester@example.org");

        }


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Api", "HslController", methodName, testScenario, testCase) {
            }
        }


        /*
         * ODATA REQUIRES WORKAROUNDS IN .NET CORE 3.1, AND BREAKS SWAGGER UI
        [Theory]
        [TestJson_("GetOData", "ReadonlyEndpointTests|FilterSkipTop", "A")]
        [TestJson_("GetOData", "ReadonlyEndpointTests|FilterOrderBySelectTop", "B")]
        public void GetOData(string t, JsonTestCase jsonTestCase) {
            var ea = GetOData_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }
        */


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
        [TestJson_("GetSingleFromStoredProcedure", "ReadonlyEndpointTests|HslByColorName", "A")]
        [TestJson_("GetSingleFromStoredProcedure", "ReadonlyEndpointTests|HslByColorName", "B")]
        public void GetSingleFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetSingleFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetSingleFromStoredProcedure", "ReadonlyEndpointTests|HslByColorName", "A")]
        [TestJson_("GetSingleFromStoredProcedure", "ReadonlyEndpointTests|HslByColorName", "B")]
        public async Task GetSingleFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetSingleFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetSingleFromJsonStoredProcedure", "ReadonlyEndpointTests|HslJsonByColorName", "A")]
        [TestJson_("GetSingleFromJsonStoredProcedure", "ReadonlyEndpointTests|HslJsonByColorName", "B")]
        public void GetSingleFromJsonStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetSingleFromJsonStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetSingleFromJsonStoredProcedure", "ReadonlyEndpointTests|HslJsonByColorName", "A")]
        [TestJson_("GetSingleFromJsonStoredProcedure", "ReadonlyEndpointTests|HslJsonByColorName", "B")]
        public async Task GetSingleFromJsonStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetSingleFromJsonStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetListFromStoredProcedure", "ReadonlyEndpointTests|HslByColorNameContains", "A")]
        [TestJson_("GetListFromStoredProcedure", "ReadonlyEndpointTests|HslByColorNameContains", "B")]
        public void GetListFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetListFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetListFromStoredProcedure", "ReadonlyEndpointTests|HslByColorNameContains", "A")]
        [TestJson_("GetListFromStoredProcedure", "ReadonlyEndpointTests|HslByColorNameContains", "B")]
        public async Task GetListFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetListFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetListFromJsonStoredProcedure", "ReadonlyEndpointTests|HslJsonByColorNameContains", "A")]
        [TestJson_("GetListFromJsonStoredProcedure", "ReadonlyEndpointTests|HslJsonByColorNameContains", "B")]
        public void GetListFromJsonStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetListFromJsonStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetListFromJsonStoredProcedure", "ReadonlyEndpointTests|HslJsonByColorNameContains", "A")]
        [TestJson_("GetListFromJsonStoredProcedure", "ReadonlyEndpointTests|HslJsonByColorNameContains", "B")]
        public async Task GetListFromJsonStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetListFromJsonStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }



    }
}
