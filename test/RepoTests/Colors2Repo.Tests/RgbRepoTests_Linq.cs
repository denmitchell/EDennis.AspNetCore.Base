using Colors2.Models;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Colors2Repo.Tests {

    [Collection("Repo Tests")]
    public class RgbRepoTests_Linq
        : RepoTests<RgbRepo, Rgb, Color2DbContext> {

        public RgbRepoTests_Linq(ITestOutputHelper output)
            : base(output) { }

        protected string[] PropertiesToIgnore { get; }
            = new string[] { "SysStart", "SysEnd" };


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Repo", "RgbRepo",
                      methodName, testScenario, testCase) {
            }
        }



        [Theory]
        [TestJson_("GetWithDynamicLinq", "Verifying with Dynamic Linq, With Select", "A")]
        [TestJson_("GetWithDynamicLinq", "Verifying with Dynamic Linq, Without Select", "B")]
        [TestJson_("GetWithDynamicLinq", "Verifying with Dynamic Linq, Exception", "C")]
        public void GetWithDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetWithDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("GetWithDynamicLinq", "Verifying with Dynamic Linq, With Select", "A")]
        [TestJson_("GetWithDynamicLinq", "Verifying with Dynamic Linq, Without Select", "B")]
        [TestJson_("GetWithDynamicLinq", "Verifying with Dynamic Linq, Exception", "C")]
        public async Task GetWithDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetWithDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("Get", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Get", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Get", "Verifying with Dynamic Linq, Null", "C")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            var ea = Get_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Get", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Get", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Get", "Verifying with Dynamic Linq, Null", "C")]
        public async Task GetAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("Delete", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Delete", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Delete", "Verifying with Dynamic Linq, Exception", "C")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            var ea = Delete_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Delete", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Delete", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Delete", "Verifying with Dynamic Linq, Exception", "C")]
        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await DeleteAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("Create", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Create", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Create", "Verifying with Dynamic Linq, Exception", "C")]
        public void Create(string t, JsonTestCase jsonTestCase) {
            var ea = Create_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Create", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Create", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Create", "Verifying with Dynamic Linq, Exception", "C")]
        public async Task CreateAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await CreateAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("Update", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Update", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Update", "Verifying with Dynamic Linq, Exception", "C")]
        public void Update(string t, JsonTestCase jsonTestCase) {
            var ea = Update_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Update", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Update", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Update", "Verifying with Dynamic Linq, Exception", "C")]
        public async Task UpdateAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await UpdateAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Patch", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Patch", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Patch", "Verifying with Dynamic Linq, Exception", "C")]
        [TestJson_("Patch", "Verifying with Dynamic Linq, Exception", "D")]
        public void Patch(string t, JsonTestCase jsonTestCase) {
            var ea = Patch_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("Patch", "Verifying with Dynamic Linq, Success", "A")]
        [TestJson_("Patch", "Verifying with Dynamic Linq, Success", "B")]
        [TestJson_("Patch", "Verifying with Dynamic Linq, Exception", "C")]
        [TestJson_("Patch", "Verifying with Dynamic Linq, Exception", "D")]
        public async Task PatchAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await PatchAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("RgbByColorName", "Verifying with Dynamic Linq, Params", "A")]
        [TestJson_("RgbByColorName", "Verifying with Dynamic Linq, Params", "B")]
        public void GetJsonObjectFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetJsonObjectFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("RgbByColorName", "Verifying with Dynamic Linq, Params", "A")]
        [TestJson_("RgbByColorName", "Verifying with Dynamic Linq, Params", "B")]
        public async Task GetJsonObjectFromStoredProcedure_Async(string t, JsonTestCase jsonTestCase) {
            var ea = await GetJsonObjectFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("RgbByColorNameContains", "Verifying with Dynamic Linq, Params", "A")]
        [TestJson_("RgbByColorNameContains", "Verifying with Dynamic Linq, Params", "B")]
        public void GetJsonArrayFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetJsonArrayFromStoredProcedure_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("RgbByColorNameContains", "Verifying with Dynamic Linq, Params", "A")]
        [TestJson_("RgbByColorNameContains", "Verifying with Dynamic Linq, Params", "B")]
        public async Task GetJsonArrayFromStoredProcedure_Async(string t, JsonTestCase jsonTestCase) {
            var ea = await GetJsonArrayFromStoredProcedureAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }


        [Theory]
        [TestJson_("ColorNameById", "Verifying with Dynamic Linq, Params", "A")]
        [TestJson_("ColorNameById", "Verifying with Dynamic Linq, Params", "B")]
        [TestJson_("ColorNameById", "Verifying with Dynamic Linq, Exception", "C")]
        [TestJson_("ColorNameById", "Verifying with Dynamic Linq, Exception", "D")]
        public void GetScalarFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
            var ea = GetScalarFromStoredProcedure_ExpectedActual<string>(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }

        [Theory]
        [TestJson_("ColorNameById", "Verifying with Dynamic Linq, Params", "A")]
        [TestJson_("ColorNameById", "Verifying with Dynamic Linq, Params", "B")]
        [TestJson_("ColorNameById", "Verifying with Dynamic Linq, Exception", "C")]
        [TestJson_("ColorNameById", "Verifying with Dynamic Linq, Exception", "D")]
        public async Task GetScalarFromStoredProcedure_Async(string t, JsonTestCase jsonTestCase) {
            var ea = await GetScalarFromStoredProcedureAsync_ExpectedActual<string>(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, PropertiesToIgnore, Output));
        }



    }
}
