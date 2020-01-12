using Colors2.Models;
using Colors2Api.Lib.Controllers;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Colors2Api.Tests
{

    [Collection("Controller Tests")]
    public class ReadonlyControllerTests : SqlServerReadonlyControllerTests<HslController, HslRepo, Hsl, Color2DbContext> {
        public ReadonlyControllerTests(ITestOutputHelper output) : base(output) {
        }

        public readonly string[] propsToIgnore = new string[] { "SysStart", "SysEnd" };

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Api", "HslController", methodName, testScenario, testCase) {
            }
        }


        //NOTE: OData is endpoint testable only
        //NOTE: GetFromStoredProcedure is endpoint testable only
        //NOTE: GetJsonColumnFromStoredProcedure is endpoint testable only


        [Theory]
        [TestJson_("GetDevExtreme", "ReadonlyControllerTests|FilterSkipTake", "A")]
        [TestJson_("GetDevExtreme", "ReadonlyControllerTests|FilterSortSelectTake", "B")]
        public void GetDevExtreme(string t, JsonTestCase jsonTestCase) {
            var ea = GetDevExtreme_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "ReadonlyControllerTests|WhereSkipTake", "A")] //RedGt200Skip2Take5
        [TestJson_("GetDynamicLinq", "ReadonlyControllerTests|WhereOrderBySelectTake", "B")] //NameContainsBlueSelectNameDescSysUserTake10
        public void GetDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "ReadonlyControllerTests|WhereSkipTake", "A")] //RedGt200Skip2Take5
        [TestJson_("GetDynamicLinq", "ReadonlyControllerTests|WhereOrderBySelectTake", "B")] //NameContainsBlueSelectNameDescSysUserTake10
        public async Task GetDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


    }
}
