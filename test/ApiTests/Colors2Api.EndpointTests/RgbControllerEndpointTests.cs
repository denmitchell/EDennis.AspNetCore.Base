using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        string[] propertiesToIgnore = new string[] { "SysStart", "SysEnd" };

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Api", "RgbController", methodName, testScenario, testCase) {
            }
        }

        [Theory]
        [TestJson_("GetWithDevExtreme", "FilterSortSelectTake", "A")]
        [TestJson_("GetWithDevExtreme", "FilterSkipTake", "B")]
        [TestJson_("GetWithDevExtreme", "Bad Request", "C")]
        public void GetWithDevExtreme(string t, JsonTestCase jsonTestCase) {
            var ea = GetWithDevExtreme_ExpectedActual(t, jsonTestCase);

            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetWithDevExtreme", "FilterSortSelectTake", "A")]
        [TestJson_("GetWithDevExtreme", "FilterSkipTake", "B")]
        [TestJson_("GetWithDevExtreme", "Bad Request", "C")]
        public async Task GetWithDevExtremeAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetWithDevExtremeAsync_ExpectedActual(t, jsonTestCase);

            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetWithDynamicLinq", "With Select", "A")]
        [TestJson_("GetWithDynamicLinq", "Without Select", "B")]
        [TestJson_("GetWithDynamicLinq", "Bad Request", "C")]
        public void GetWithDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetWithDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetWithDynamicLinq", "With Select", "A")]
        [TestJson_("GetWithDynamicLinq", "Without Select", "B")]
        [TestJson_("GetWithDynamicLinq", "Bad Request", "C")]
        public async Task GetWithDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetWithDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Get", "Success", "A")]
        [TestJson_("Get", "Success", "B")]
        [TestJson_("Get", "Not Found", "C")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            var ea = Get_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("Get", "Success", "A")]
        [TestJson_("Get", "Success", "B")]
        [TestJson_("Get", "Not Found", "C")]
        public async Task GetAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Delete", "No Content", "A")]
        [TestJson_("Delete", "No Content", "B")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            var ea = Delete_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Delete", "No Content", "A")]
        [TestJson_("Delete", "No Content", "B")]
        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await DeleteAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("Post", "Success", "A")]
        [TestJson_("Post", "Success", "B")]
        [TestJson_("Post", "Conflict", "C")]
        public void Post(string t, JsonTestCase jsonTestCase) {
            var ea = Post_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Post", "Success", "A")]
        [TestJson_("Post", "Success", "B")]
        [TestJson_("Post", "Conflict", "C")]
        public async Task PostAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PostAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("Put", "Success", "A")]
        [TestJson_("Put", "Success", "B")]
        [TestJson_("Put", "Bad Request - Bad Id", "C")]
        public void Put(string t, JsonTestCase jsonTestCase) {
            var ea = Put_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Put", "Success", "A")]
        [TestJson_("Put", "Success", "B")]
        [TestJson_("Put", "Bad Request - Bad Id", "C")]
        public async Task PutAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PutAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Patch", "Success", "A")]
        [TestJson_("Patch", "Success", "B")]
        [TestJson_("Patch", "Bad Request - Bad Id", "C")]
        [TestJson_("Patch", "Bad Request - Not Deserializable", "D")]
        public void Patch(string t, JsonTestCase jsonTestCase) {
            var ea = Patch_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Patch", "Success", "A")]
        [TestJson_("Patch", "Success", "B")]
        [TestJson_("Patch", "Bad Request - Bad Id", "C")]
        [TestJson_("Patch", "Bad Request - Not Deserializable", "D")]
        public async Task PatchAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PatchAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }





    }
}
