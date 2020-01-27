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
        public void GetDevExtreme(string t, JsonTestCase jsonTestCase) {
            var ea = GetDevExtreme_ExpectedActual(t, jsonTestCase);

            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDevExtreme", "FilterSortSelectTake", "A")]
        [TestJson_("GetDevExtreme", "FilterSkipTake", "B")]
        public async Task GetDevExtremeAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetDevExtremeAsync_ExpectedActual(t, jsonTestCase);

            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "WithSelect", "A")]
        [TestJson_("GetDynamicLinq", "WithoutSelect", "B")]
        public void GetDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "WithSelect", "A")]
        [TestJson_("GetDynamicLinq", "WithoutSelect", "B")]
        public async Task GetDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetById", "", "A")]
        [TestJson_("GetById", "", "B")]
        public void GetById(string t, JsonTestCase jsonTestCase) {
            var ea = GetById_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("GetById", "", "A")]
        [TestJson_("GetById", "", "B")]
        public async Task GetByIdAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetByIdAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Delete", "", "A")]
        [TestJson_("Delete", "", "B")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            var ea = Delete_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Delete", "", "A")]
        [TestJson_("Delete", "", "B")]
        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await DeleteAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Post", "", "A")]
        [TestJson_("Post", "", "B")]
        public void Post(string t, JsonTestCase jsonTestCase) {
            var ea = Post_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Post", "", "A")]
        [TestJson_("Post", "", "B")]
        public async Task PostAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PostAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Put", "", "A")]
        [TestJson_("Put", "", "B")]
        public void Put(string t, JsonTestCase jsonTestCase) {
            var ea = Put_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Put", "", "A")]
        [TestJson_("Put", "", "B")]
        public async Task PutAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PutAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Patch", "", "A")]
        [TestJson_("Patch", "", "B")]
        public void Patch(string t, JsonTestCase jsonTestCase) {
            var ea = Patch_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Patch", "", "A")]
        [TestJson_("Patch", "", "B")]
        public async Task PatchAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PatchAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

    }
}
