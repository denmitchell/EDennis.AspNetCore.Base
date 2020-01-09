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
    public class RgbRepoTests
        : RepoTests<RgbRepo, Rgb, Color2DbContext> {

        public RgbRepoTests(ITestOutputHelper output)
            : base(output) { }

        protected string[] PropertiesToIgnore { get; }
            = new string[] { "SysStart", "SysEnd" };

        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Repo", "RgbRepo",
                      methodName, testScenario, testCase) {
            }
        }

        [Fact]
        public void GetTestJsonForProject() {
            var attr = new TestJsonAttribute("Color2Db", "Colors2Repo", "RgbRepo", "Create", "", "A");
            var cases = attr.GetData(null);
            Assert.NotNull(cases);
        }


        [Theory]
        [TestJsonA("Create", "", "A")]
        [TestJsonA("Create", "", "B")]
        public void Create(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var input = jsonTestCase.GetObject<Rgb>("Input");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");

            Repo.Create(input);

            var actual = Repo.Query.Where(e => e.Id <= start).ToList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        [Theory]
        [TestJsonA("Create", "", "A")]
        [TestJsonA("Create", "", "B")]
        public async Task CreateAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var input = jsonTestCase.GetObject<Rgb>("Input");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");

            await Repo.CreateAsync(input);

            var actual = Repo.Query.Where(e => e.Id <= start).ToList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        [Theory]
        [TestJsonA("Update", "", "A")]
        [TestJsonA("Update", "", "B")]
        public void Update(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var input = jsonTestCase.GetObject<dynamic>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");

            Repo.Update(input, id);

            var actual = Repo.Query.Where(e => e.Id <= start).ToList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        [Theory]
        [TestJsonA("Update", "", "A")]
        [TestJsonA("Update", "", "B")]
        public async Task UpdateAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var input = jsonTestCase.GetObject<dynamic>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");

            await Repo.UpdateAsync(input, id);

            var actual = Repo.Query.Where(e => e.Id <= start).ToList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJsonA("Delete", "", "A")]
        [TestJsonA("Delete", "", "B")]
        public void Delete(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");

            Repo.Delete(id);

            var actual = Repo.Query.Where(e => e.Id <= start).ToList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJsonA("Delete", "", "A")]
        [TestJsonA("Delete", "", "B")]
        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");

            await Repo.DeleteAsync(id);

            var actual = Repo.Query.Where(e => e.Id <= start).ToList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        [Theory]
        [TestJsonA("GetById", "", "A")]
        [TestJsonA("GetById", "", "B")]
        public void GetById(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Rgb>("Expected");

            var actual = Repo.GetById(id);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJsonA("GetById", "", "A")]
        [TestJsonA("GetById", "", "B")]
        public async Task GetByIdAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Rgb>("Expected");

            var actual = await Repo.GetByIdAsync(id);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJsonA("GetFromDynamicLinq", "WhereOrderBySelectTake", "A")]
        [TestJsonA("GetFromDynamicLinq", "WhereOrderBySkipTake", "B")]
        public void GetFromDynamicLinq(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.TestScenario.Contains("Where") ? jsonTestCase.GetObject<string>("Where") : null;
            string orderBy = jsonTestCase.TestScenario.Contains("OrderBy") ? jsonTestCase.GetObject<string>("OrderBy") : null;
            string select = jsonTestCase.TestScenario.Contains("Select") ? jsonTestCase.GetObject<string>("Select") : null;
            int? skip = jsonTestCase.TestScenario.Contains("Skip") ? jsonTestCase.GetObject<int?>("Skip") : null;
            int? take = jsonTestCase.TestScenario.Contains("Take") ? jsonTestCase.GetObject<int?>("Take") : null;


            var expected  = DynamicConverter.ToPropertyDictionaryList(jsonTestCase.GetObject<List<dynamic>>("Expected"));

            var actual = DynamicConverter.ToPropertyDictionaryList(Repo.GetFromDynamicLinq(where, orderBy, select, skip, take));

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual,expected, 3, PropertiesToIgnore, Output, true));

        }


        [Theory]
        [TestJsonA("GetFromDynamicLinq", "WhereOrderBySelectTake", "A")]
        [TestJsonA("GetFromDynamicLinq", "WhereOrderBySkipTake", "B")]
        public async Task GetFromDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.TestScenario.Contains("Where") ? jsonTestCase.GetObject<string>("Where") : null;
            string orderBy = jsonTestCase.TestScenario.Contains("OrderBy") ? jsonTestCase.GetObject<string>("OrderBy") : null;
            string select = jsonTestCase.TestScenario.Contains("Select") ? jsonTestCase.GetObject<string>("Select") : null;
            int? skip = jsonTestCase.TestScenario.Contains("Skip") ? jsonTestCase.GetObject<int?>("Skip") : null;
            int? take = jsonTestCase.TestScenario.Contains("Take") ? jsonTestCase.GetObject<int?>("Take") : null;


            var expected = DynamicConverter.ToPropertyDictionaryList(jsonTestCase.GetObject<List<dynamic>>("Expected"));

            var actual = DynamicConverter.ToPropertyDictionaryList(await Repo.GetFromDynamicLinqAsync(where, orderBy, select, skip, take));

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, true));

        }


        [Theory]
        [TestJsonA("GetFromJsonSql", "", "A")]
        [TestJsonA("GetFromJsonSql", "", "B")]
        public void GetFromJsonSql(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var sql = jsonTestCase.GetObject<string>("Sql");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

            var actualJson = Repo.GetFromJsonSql(sql);
            var actual = JsonSerializer.Deserialize<List<Rgb>>(actualJson);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }



        [Theory]
        [TestJsonA("GetFromJsonSql", "", "A")]
        [TestJsonA("GetFromJsonSql", "", "B")]
        public async Task GetFromJsonSqlAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var sql = jsonTestCase.GetObject<string>("Sql");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

            var actualJson = await Repo.GetFromJsonSqlAsync(sql);
            var actual = JsonSerializer.Deserialize<List<Rgb>>(actualJson);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }



        [Theory]
        [TestJsonA("GetFromSql", "", "A")]
        [TestJsonA("GetFromSql", "", "B")]
        public void GetFromSql(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var sql = jsonTestCase.GetObject<string>("Sql");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

            var actual = Repo.GetFromSql(sql);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }



        [Theory]
        [TestJsonA("GetFromSql", "", "A")]
        [TestJsonA("GetFromSql", "", "B")]
        public async Task GetFromSqlAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var sql = jsonTestCase.GetObject<string>("Sql");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

            var actual = await Repo.GetFromSqlAsync(sql);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJsonA("GetScalarFromSql", "", "A")]
        [TestJsonA("GetScalarFromSql", "", "B")]
        public void GetScalarFromSql(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var sql = jsonTestCase.GetObject<string>("Sql");
            var expected = jsonTestCase.GetObject<int>("Expected");

            var actual = Repo.GetScalarFromSql<int>(sql);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJsonA("GetScalarFromSql", "", "A")]
        [TestJsonA("GetScalarFromSql", "", "B")]
        public async Task GetScalarFromSqlAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var sql = jsonTestCase.GetObject<string>("Sql");
            var expected = jsonTestCase.GetObject<int>("Expected");

            var actual = await Repo.GetScalarFromSqlAsync<int>(sql);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJsonA("GetJsonColumnFromStoredProcedure", "", "A")]
        [TestJsonA("GetJsonColumnFromStoredProcedure", "", "B")]
        public void GetJsonColumnFromStoredProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new List<KeyValuePair<string, string>> { KeyValuePair.Create("ColorName", colorName) };


            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<Rgb>(expectedJson);

            var actualJson = Repo.GetJsonColumnFromStoredProcedure(spName, parameters);
            var actual = JsonSerializer.Deserialize<Rgb>(actualJson);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }



        [Theory]
        [TestJsonA("GetJsonColumnFromStoredProcedure", "", "A")]
        [TestJsonA("GetJsonColumnFromStoredProcedure", "", "B")]
        public async Task GetJsonColumnFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new List<KeyValuePair<string, string>> { KeyValuePair.Create("ColorName", colorName) };


            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<Rgb>(expectedJson);

            var actualJson = await Repo.GetJsonColumnFromStoredProcedureAsync(spName, parameters);
            var actual = JsonSerializer.Deserialize<Rgb>(actualJson);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }



    }

}
