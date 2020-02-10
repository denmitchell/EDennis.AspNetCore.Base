using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Hr.RepoApi.Models;
using Xunit;
using Xunit.Abstractions;


namespace Hr.RepoApi.Tests {
    public class AddressRepoTests : RepoTests<AddressRepo, Address, HrContext> {
        public AddressRepoTests(ITestOutputHelper output) : base(output) {
        }


        private string[] _propertiesToIgnore = new string[] { "SysStart", "SysEnd" }; 

        class TestJson_ : TestJsonAttribute {            
            public TestJson_(string methodName, string testScenario, string testCase, string serverName = "(LocalDb)\\MSSQLLocalDb", string testJsonSchema = "_", string testJsonTable = "TestJson") 
                : base("Hr123", "Hr.RepoApi.Lib", "AddressRepo", methodName, testScenario, testCase, serverName, testJsonSchema, testJsonTable){ }
        }

        [Theory]
        [TestJson_("Create","Success","A")]
        public void Create(string t, JsonTestCase jsonTestCase) {
            var ea = Create_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, _propertiesToIgnore, Output, true));
        }

        [Theory]
        [TestJson_("Patch", "Success", "A")]
        public void Patch(string t, JsonTestCase jsonTestCase) {
            var ea = Patch_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, _propertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJson_("Delete", "Success", "A")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            var ea = Delete_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, _propertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJson_("Get", "Success", "A")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            var ea = Get_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, _propertiesToIgnore, Output, true));
        }



    }
}
