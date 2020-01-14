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
    public class WriteableEndpointTests 
        : SqlServerWriteableEndpointTests<Hsl, L.Program, Colors2ApiLauncher> {

        public WriteableEndpointTests(ITestOutputHelper output,
            LauncherFixture<L.Program, Colors2ApiLauncher> launcherFixture)
            : base(output, launcherFixture) {

            if (HttpClient.DefaultRequestHeaders.Contains("X-User"))
                HttpClient.DefaultRequestHeaders.Remove("X-User");
            HttpClient.DefaultRequestHeaders.Add("X-User", "tester@example.org");
        }


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Api", "RgbController", methodName, testScenario, testCase) {
            }
        }

        #region (from readonly)

        [Theory]
        [TestJson_("GetDevExtreme", "WriteableEndpointTests|FilterSortSelectTake", "A")]
        [TestJson_("GetDevExtreme", "WriteableEndpointTests|FilterSkipTake", "B")]
        public void GetDevExtreme(string t, JsonTestCase jsonTestCase) {
            var ea = GetDevExtreme_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "WriteableEndpointTests|WhereOrderBySelectTake", "A")]
        [TestJson_("GetDynamicLinq", "WriteableEndpointTests|WhereSkipTake", "B")]
        public void GetDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "WriteableEndpointTests|WhereOrderBySelectTake", "A")]
        [TestJson_("GetDynamicLinq", "WriteableEndpointTests|WhereSkipTake", "B")]
        public async Task GetDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetSingleFromStoredProcedure", "WriteableEndpointTests|RgbByColorName", "A")]
        [TestJson_("GetSingleFromStoredProcedure", "WriteableEndpointTests|RgbByColorName", "B")]
        public void GetSingleFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetSingleFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetSingleFromStoredProcedure", "WriteableEndpointTests|RgbByColorName", "A")]
        [TestJson_("GetSingleFromStoredProcedure", "WriteableEndpointTests|RgbByColorName", "B")]
        public async Task GetSingleFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetSingleFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetSingleFromJsonStoredProcedure", "WriteableEndpointTests|RgbJsonByColorName", "A")]
        [TestJson_("GetSingleFromJsonStoredProcedure", "WriteableEndpointTests|RgbJsonByColorName", "B")]
        public void GetSingleFromJsonStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetSingleFromJsonStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetSingleFromJsonStoredProcedure", "WriteableEndpointTests|RgbJsonByColorName", "A")]
        [TestJson_("GetSingleFromJsonStoredProcedure", "WriteableEndpointTests|RgbJsonByColorName", "B")]
        public async Task GetSingleFromJsonStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetSingleFromJsonStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetListFromStoredProcedure", "WriteableEndpointTests|RgbByColorNameContains", "A")]
        [TestJson_("GetListFromStoredProcedure", "WriteableEndpointTests|RgbByColorNameContains", "B")]
        public void GetListFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetListFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetListFromStoredProcedure", "WriteableEndpointTests|RgbByColorNameContains", "A")]
        [TestJson_("GetListFromStoredProcedure", "WriteableEndpointTests|RgbByColorNameContains", "B")]
        public async Task GetListFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetListFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetListFromJsonStoredProcedure", "WriteableEndpointTests|RgbJsonByColorNameContains", "A")]
        [TestJson_("GetListFromJsonStoredProcedure", "WriteableEndpointTests|RgbJsonByColorNameContains", "B")]
        public void GetListFromJsonStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetListFromJsonStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetListFromJsonStoredProcedure", "WriteableEndpointTests|RgbJsonByColorNameContains", "A")]
        [TestJson_("GetListFromJsonStoredProcedure", "WriteableEndpointTests|RgbJsonByColorNameContains", "B")]
        public async Task GetListFromJsonStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetListFromJsonStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        #endregion


        [Theory]
        [TestJson_("Get", "WriteableEndpointTests", "A")]
        [TestJson_("Get", "WriteableEndpointTests", "B")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            var ea = Get_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Get", "WriteableEndpointTests", "A")]
        [TestJson_("Get", "WriteableEndpointTests", "B")]
        public async Task GetAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Delete", "WriteableEndpointTests", "A")]
        [TestJson_("Delete", "WriteableEndpointTests", "B")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            var ea = Delete_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Delete", "WriteableEndpointTests", "A")]
        [TestJson_("Delete", "WriteableEndpointTests", "B")]
        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await DeleteAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Post", "WriteableEndpointTests", "A")]
        [TestJson_("Post", "WriteableEndpointTests", "B")]
        public void Post(string t, JsonTestCase jsonTestCase) {
            var ea = Post_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Post", "WriteableEndpointTests", "A")]
        [TestJson_("Post", "WriteableEndpointTests", "B")]
        public async Task PostAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PostAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Put", "WriteableEndpointTests", "A")]
        [TestJson_("Put", "WriteableEndpointTests", "B")]
        public void Put(string t, JsonTestCase jsonTestCase) {
            var ea = Put_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Put", "WriteableEndpointTests", "A")]
        [TestJson_("Put", "WriteableEndpointTests", "B")]
        public async Task PutAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PutAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }



        [Theory]
        [TestJson_("Put", "WriteableEndpointTests", "A")]
        [TestJson_("Put", "WriteableEndpointTests", "B")]
        public void Patch(string t, JsonTestCase jsonTestCase) {
            var ea = Patch_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Put", "WriteableEndpointTests", "A")]
        [TestJson_("Put", "WriteableEndpointTests", "B")]
        public async Task PatchAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PatchAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

    }
}
