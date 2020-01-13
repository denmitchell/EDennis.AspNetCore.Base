using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using System.Text.Json;

namespace RepoTests {
    public class HslRepoTests
        : RepoTests<HslRepo, Hsl, Color2DbContext> {

        public HslRepoTests(ITestOutputHelper output)
            : base(output) { }

        protected string[] PropertiesToIgnore { get; }
            = new string[] { "SysStart", "SysEnd" };

        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Repo", "HslRepo",
                      methodName, testScenario, testCase) {
            }
        }

        [Theory]
        [TestJsonA("GetFromStoredProcedure", "", "A")]
        [TestJsonA("GetFromStoredProcedure", "", "B")]
        public void GetFromStoreProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new List<KeyValuePair<string, string>> { KeyValuePair.Create("ColorName", colorName) };


            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = DynamicConverter.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamic = Repo.Context.GetFromStoredProcedure(spName, parameters);
            var actual = DynamicConverter.ToPropertyDictionaryList(actualDynamic);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }



        [Theory]
        [TestJsonA("GetFromStoredProcedure", "", "A")]
        [TestJsonA("GetFromStoredProcedure", "", "B")]
        public async Task GetFromStoreProcedureAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new List<KeyValuePair<string, string>> { KeyValuePair.Create("ColorName", colorName) };


            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = DynamicConverter.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamic = await Repo.Context.GetFromStoredProcedureAsync(spName, parameters);
            var actual = DynamicConverter.ToPropertyDictionaryList(actualDynamic);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


    }
}
