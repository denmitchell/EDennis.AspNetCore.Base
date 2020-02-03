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
    public class ProcControllerEndpointTests 
        : SqlServerStoredProcedureControllerEndpointTests<Color2DbContext, L.Program, Colors2ApiLauncher> {

        public ProcControllerEndpointTests(ITestOutputHelper output,
            LauncherFixture<L.Program, Colors2ApiLauncher> launcherFixture)
            : base(output, launcherFixture) {

            if (HttpClient.DefaultRequestHeaders.Contains("X-User"))
                HttpClient.DefaultRequestHeaders.Remove("X-User");
            HttpClient.DefaultRequestHeaders.Add("X-User", "tester@example.org");
        }

        private readonly string[] _propertiesToIgnore = new string[] { "SysStart", "SysEnd" };

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Api", "ProcController", methodName, testScenario, testCase) {
            }
        }



        [Theory]
        [TestJson_("GetJsonObjectFromStoredProcedure", "Success", "A")]
        [TestJson_("GetJsonObjectFromStoredProcedure", "Success", "B")]
        [TestJson_("GetJsonObjectFromStoredProcedure", "Bad Request", "C")]
        public void GetJsonObjectFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetJsonObjectFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ObjectExtensions.IsEqualAndWrite(ea.Expected, ea.Actual, Output, _propertiesToIgnore, true, false));
        }

        [Theory]
        [TestJson_("GetJsonObjectFromStoredProcedure", "Success", "A")]
        [TestJson_("GetJsonObjectFromStoredProcedure", "Success", "B")]
        [TestJson_("GetJsonObjectFromStoredProcedure", "Bad Request", "C")]
        public async Task GetJsonObjectFromStoredProcedure_Async(string t, JsonTestCase jsonTestCase) {
            var ea = await GetJsonObjectFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ObjectExtensions.IsEqualAndWrite(ea.Expected, ea.Actual, Output, _propertiesToIgnore, true, false));
        }


        [Theory]
        [TestJson_("GetJsonArrayFromStoredProcedure", "Success", "A")]
        [TestJson_("GetJsonArrayFromStoredProcedure", "Success", "B")]
        [TestJson_("GetJsonArrayFromStoredProcedure", "Bad Request", "C")]
        public void GetJsonArrayFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetJsonArrayFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ObjectExtensions.IsEqualAndWrite(ea.Expected,ea.Actual,Output,_propertiesToIgnore,true,false));
        }

        [Theory]
        [TestJson_("GetJsonArrayFromStoredProcedure", "Success", "A")]
        [TestJson_("GetJsonArrayFromStoredProcedure", "Success", "B")]
        [TestJson_("GetJsonArrayFromStoredProcedure", "Bad Request", "C")]
        public async Task GetJsonArrayFromStoredProcedure_Async(string t, JsonTestCase jsonTestCase) {
            var ea = await GetJsonArrayFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ObjectExtensions.IsEqualAndWrite(ea.Expected, ea.Actual, Output, _propertiesToIgnore, true, false));
        }


        [Theory]
        [TestJson_("GetJsonFromJsonStoredProcedure", "Success", "A")]
        [TestJson_("GetJsonFromJsonStoredProcedure", "Success", "B")]
        [TestJson_("GetJsonFromJsonStoredProcedure", "Bad Request", "C")]
        public void GetJsonFromJsonStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetJsonFromJsonStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ObjectExtensions.IsEqualAndWrite(ea.Expected, ea.Actual, Output, _propertiesToIgnore, true, false));
        }

        [Theory]
        [TestJson_("GetJsonFromJsonStoredProcedure", "Success", "A")]
        [TestJson_("GetJsonFromJsonStoredProcedure", "Success", "B")]
        [TestJson_("GetJsonFromJsonStoredProcedure", "Bad Request", "C")]
        public async Task GetJsonFromJsonStoredProcedure_Async(string t, JsonTestCase jsonTestCase) {
            var ea = await GetJsonFromJsonStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ObjectExtensions.IsEqualAndWrite(ea.Expected, ea.Actual, Output, _propertiesToIgnore, true, false));
        }


        [Theory]
        [TestJson_("GetScalarFromStoredProcedure", "Success", "A")]
        [TestJson_("GetScalarFromStoredProcedure", "Success", "B")]
        [TestJson_("GetScalarFromStoredProcedure", "Bad Request", "C")]
        public void GetScalarStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetScalarFromStoredProcedure_ExpectedActual<int>(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, _propertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("GetScalarFromStoredProcedure", "Success", "A")]
        [TestJson_("GetScalarFromStoredProcedure", "Success", "B")]
        [TestJson_("GetScalarFromStoredProcedure", "Bad Request", "C")]
        public async Task GetScalarStoredProcedure_Async(string t, JsonTestCase jsonTestCase) {
            var ea = await GetScalarFromStoredProcedureAsync_ExpectedActual<int>(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, _propertiesToIgnore, Output));
        }

    }
}
