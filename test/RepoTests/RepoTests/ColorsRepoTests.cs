using Colors.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace RepoTests {
    public class ColorsRepoTests
        : TemporalRepoTests<ColorRepo, Color, ColorHistory, ColorDbContext, ColorHistoryDbContext>{
        
        public ColorsRepoTests(ITestOutputHelper output, TestTemporalRepoFixture<ColorRepo, Color, ColorHistory, ColorDbContext, ColorHistoryDbContext> fixture) 
            : base(output, fixture) { }

        protected string[] propertiesToIgnore { get; }
            = new string[] { "SysStart", "SysEnd" };

        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("ColorsRepo", "ColorRepo",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "ColorsRepo\\TestJson.xlsx") {
            }
        }

        private IEnumerable<Color> BaseExpected(JsonTestCase jsonTestCase) {
            return null;
        }

        private void Seed(JsonTestCase jsonTestCase) {
            var inputs = jsonTestCase.GetObject<List<Color>>($"Seed");
            foreach (var input in inputs)
                Repo.Create(input);
        }


        [Theory]
        [TestJsonA("Create", "", "A")]
        [TestJsonA("Create", "", "B")]
        public void Create(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");

            Repo.Create(input);

            var actual = Repo.Query.ToPagedList();

            Assert.True(actual.IsEqualOrWrite(expected,3,propertiesToIgnore,Output,true));
        }

    }
}
