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
    public class RgbControllerEndpointTests 
        : RepoControllerEndpointTests<Rgb, L.Program, Colors2ApiLauncher> {

        public RgbControllerEndpointTests(ITestOutputHelper output,
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

        [Theory]
        [TestJson_("GetDevExtreme", "FilterSortSelectTake", "A")]
        [TestJson_("GetDevExtreme", "FilterSkipTake", "B")]
        [TestJson_("GetDevExtreme", "Bad Request", "C")]
        public void GetDevExtreme(string t, JsonTestCase jsonTestCase) {
            var ea = GetDevExtreme_ExpectedActual(t, jsonTestCase);

            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDevExtreme", "FilterSortSelectTake", "A")]
        [TestJson_("GetDevExtreme", "FilterSkipTake", "B")]
        [TestJson_("GetDevExtreme", "Bad Request", "C")]
        public async Task GetDevExtremeAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetDevExtremeAsync_ExpectedActual(t, jsonTestCase);

            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "With Select", "A")]
        [TestJson_("GetDynamicLinq", "Without Select", "B")]
        [TestJson_("GetDynamicLinq", "Bad Request", "C")]
        public void GetDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "With Select", "A")]
        [TestJson_("GetDynamicLinq", "Without Select", "B")]
        [TestJson_("GetDynamicLinq", "Bad Request", "C")]
        public async Task GetDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetById", "Success", "A")]
        [TestJson_("GetById", "Success", "B")]
        [TestJson_("GetById", "Not Found", "C")]
        public void GetById(string t, JsonTestCase jsonTestCase) {
            var ea = GetById_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("GetById", "", "A")]
        [TestJson_("GetById", "", "B")]
        [TestJson_("GetById", "Not Found", "C")]
        public async Task GetByIdAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetByIdAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Delete", "No Content", "A")]
        [TestJson_("Delete", "No Content", "B")]
        [TestJson_("Delete", "Not Found", "C")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            var ea = Delete_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Delete", "No Content", "A")]
        [TestJson_("Delete", "No Content", "B")]
        [TestJson_("Delete", "Not Found", "C")]
        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await DeleteAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Post", "Success", "A")]
        [TestJson_("Post", "Success", "B")]
        [TestJson_("Post", "Conflict", "C")]
        public void Post(string t, JsonTestCase jsonTestCase) {
            var ea = Post_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Post", "Success", "A")]
        [TestJson_("Post", "Success", "B")]
        [TestJson_("Post", "Conflict", "C")]
        public async Task PostAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PostAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Put", "Success", "A")]
        [TestJson_("Put", "Success", "B")]
        [TestJson_("Put", "Bad Request - Bad Id", "C")]
        public void Put(string t, JsonTestCase jsonTestCase) {
            var ea = Put_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Put", "Success", "A")]
        [TestJson_("Put", "Success", "B")]
        [TestJson_("Put", "Bad Request - Bad Id", "C")]
        public async Task PutAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PutAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Patch", "Success", "A")]
        [TestJson_("Patch", "Success", "B")]
        [TestJson_("Patch", "Bad Request - Bad Id", "C")]
        [TestJson_("Patch", "Bad Request - Not Deserializable", "C")]
        public void Patch(string t, JsonTestCase jsonTestCase) {
            var ea = Patch_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Patch", "Success", "A")]
        [TestJson_("Patch", "Success", "B")]
        [TestJson_("Patch", "Bad Request - Bad Id", "C")]
        [TestJson_("Patch", "Bad Request - Not Deserializable", "C")]
        public async Task PatchAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PatchAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }




    }
}
