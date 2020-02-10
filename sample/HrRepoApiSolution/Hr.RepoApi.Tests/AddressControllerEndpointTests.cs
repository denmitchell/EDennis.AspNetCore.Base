using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Hr.RepoApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using L = Hr.RepoApi.Lib;

namespace Hr.RepoApi.Tests {

    [Collection("Endpoint Tests")]
    public class AddressControllerEndpointTests 
        : RepoControllerEndpointTests<Address, L.Program, Launcher.Launcher> {

        public AddressControllerEndpointTests(ITestOutputHelper output,
            LauncherFixture<L.Program, Launcher.Launcher> launcherFixture)
            : base(output, launcherFixture) {

            if (HttpClient.DefaultRequestHeaders.Contains("X-User"))
                HttpClient.DefaultRequestHeaders.Remove("X-User");
            HttpClient.DefaultRequestHeaders.Add("X-User", "tester@example.org");
        }

        readonly string[] propertiesToIgnore = new string[] { "SysStart", "SysEnd" };

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Hr123", "Hr.RepoApi.Lib", "AddressController", methodName, testScenario, testCase) {
            }
        }

        [Theory]
        [TestJson_("GetWithDevExtreme", "FilterSortSelectTake", "A")]
        [TestJson_("GetWithDevExtreme", "Bad Request", "C")]
        public void GetWithDevExtreme(string t, JsonTestCase jsonTestCase) {
            var ea = GetWithDevExtreme_ExpectedActual(t, jsonTestCase);

            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetWithDynamicLinq", "With Select", "A")]
        [TestJson_("GetWithDynamicLinq", "Without Select", "B")]
        public void GetWithDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetWithDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }



        [Theory]
        [TestJson_("Get", "Success", "A")]
        [TestJson_("Get", "Not Found", "C")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            var ea = Get_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Delete", "No Content", "A")]
        [TestJson_("Delete", "Not Found", "C")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            var ea = Delete_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("Post", "Success", "A")]
        [TestJson_("Post", "Conflict", "C")]
        public void Post(string t, JsonTestCase jsonTestCase) {
            var ea = Post_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("Put", "Success", "A")]
        [TestJson_("Put", "Bad Request - Bad Id", "C")]
        public void Put(string t, JsonTestCase jsonTestCase) {
            var ea = Put_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Patch", "Success", "A")]
        [TestJson_("Patch", "Bad Request - Bad Id", "C")]
        [TestJson_("Patch", "Bad Request - Not Deserializable", "D")]
        public void Patch(string t, JsonTestCase jsonTestCase) {
            var ea = Patch_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, propertiesToIgnore, Output));
        }



    }
}
