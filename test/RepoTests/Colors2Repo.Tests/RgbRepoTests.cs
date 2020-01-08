using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace RepoTests {
    public class RgbRepoTests
        : RepoTests<RgbRepo, Rgb, Color2DbContext>{
        
        public RgbRepoTests(ITestOutputHelper output) 
            : base(output) { }

        protected string[] PropertiesToIgnore { get; }
            = new string[] { "SysStart", "SysEnd" };

        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("Color2Db","Colors2Repo", "RgbRepo",
                      methodName, testScenario, testCase) {
            }
        }

        private IEnumerable<Rgb> BaseExpected(JsonTestCase jsonTestCase) {
            var readSeed = jsonTestCase.GetObject<List<Rgb>>($"ReadSeed");
            var writeSeed = jsonTestCase.GetObject<List<Rgb>>($"WriteSeed");
            return readSeed.Union(writeSeed);
        }

        private void WriteSeed(JsonTestCase jsonTestCase) {
            var inputs = jsonTestCase.GetObject<List<Rgb>>($"WriteSeed");
            foreach (var input in inputs)
                Repo.Create(input);
        }

        [Fact]
        public void GetTestJsonForProject() {
            var attr = new TestJsonAttribute("Color2Db", "ColorsRepo", "RgbRepo", "Create", "", "A");
            var cases = attr.GetData(null);
            Assert.NotNull(cases);
        }


        [Theory]
        [TestJsonA("Create", "", "A")]
        [TestJsonA("Create", "", "B")]
        public void Create(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            //WriteSeed(jsonTestCase); no need to seed for create

            var input = jsonTestCase.GetObject<Rgb>("Input");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("ReadOnlyStart");

            Repo.Create(input);

            var actual = Repo.Query.Where(e=>e.Id <=start).ToList();

            Assert.True(actual.IsEqualAndWrite(expected,3,PropertiesToIgnore,Output,true));
        }


        [Theory]
        [TestJsonA("Update", "", "A")]
        [TestJsonA("Update", "", "B")]
        public void Update(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            WriteSeed(jsonTestCase);

            var input = jsonTestCase.GetObject<Rgb>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

            Repo.Update(input,id);

            var actual = Repo.Query.ToPagedList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        [Theory]
        [TestJsonA("Delete", "", "A")]
        [TestJsonA("Delete", "", "B")]
        public void Delete(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            WriteSeed(jsonTestCase);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

            Repo.Delete(id);

            var actual = Repo.Query.ToPagedList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


    }
}
