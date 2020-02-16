using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Hr.RepoApi.Models;
using Xunit;
using Xunit.Abstractions;
using L = Hr.RepoApi.Lib;

namespace Hr.RepoApi.Tests {

    [Collection("Endpoint Tests")]
    public class ProcControllerEndpointTests 
        : SqlServerStoredProcedureControllerEndpointTests<HrContext, L.Program, Launcher.Launcher> {

        public ProcControllerEndpointTests(ITestOutputHelper output,
            LauncherFixture<L.Program, Launcher.Launcher> launcherFixture)
            : base(output, launcherFixture) {

            if (HttpClient.DefaultRequestHeaders.Contains("X-User"))
                HttpClient.DefaultRequestHeaders.Remove("X-User");
            HttpClient.DefaultRequestHeaders.Add("X-User", "tester@example.org");
        }

        private readonly string[] _propertiesToIgnore = new string[] { "SysStart", "SysEnd" };

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Hr123", "Hr.RepoApi", "ProcController", methodName, testScenario, testCase) {
            }
        }



        [Theory]
        [TestJson_("PersonsAndAddressesByState", "Success", "A")]
        public void PersonsAndAddressesByState(string t, JsonTestCase jsonTestCase) {
            var ea = GetJsonFromJsonStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ObjectExtensions.IsEqualAndWrite(ea.Expected, ea.Actual, Output, _propertiesToIgnore, true, false));
        }


    }
}
