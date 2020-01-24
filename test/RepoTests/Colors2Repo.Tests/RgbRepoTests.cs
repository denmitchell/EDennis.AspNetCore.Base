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
using EDennis.AspNetCore.Base.Serialization;
using System;
using System.Linq.Dynamic.Core;

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


        #region Create

        [Theory]
        [TestJsonA("Create", "", "A")]
        [TestJsonA("Create", "", "B")]
        public void Create(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine($"Test case: {t}");

            var input = jsonTestCase.GetObject<Rgb>("Input");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            Repo.Create(input);

            var actual = Repo.Query.Where(e => e.Id >= start && e.Id <= end).ToList();

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
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            await Repo.CreateAsync(input);

            var actual = Repo.Query.Where(e => e.Id >= start && e.Id <= end).ToList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        #endregion
        #region Update

        [Theory]
        [TestJsonA("Update", "", "A")]
        [TestJsonA("Update", "", "B")]
        public void Update(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine($"Test case: {t}");

            var input = jsonTestCase.GetObject<dynamic, Rgb>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            Repo.Update(input, id);

            var actual = Repo.Query.Where(e => e.Id >= start && e.Id <= end).ToList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJsonA("Update", "", "A")]
        [TestJsonA("Update", "", "B")]
        public async Task UpdateAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine($"Test case: {t}");

            var input = jsonTestCase.GetObject<dynamic, Rgb>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            await Repo.UpdateAsync(input, id);

            var actual = Repo.Query.Where(e => e.Id >= start && e.Id <= end).ToList();

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));

        }

        #endregion
        #region Delete

        [Theory]
        [TestJsonA("Delete", "", "A")]
        [TestJsonA("Delete", "", "B")]
        public void Delete(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            Repo.Delete(id);

            var actual = Repo.Query.Where(e => e.Id >= start && e.Id <= end).ToList();

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
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            await Repo.DeleteAsync(id);

            var actual = Repo.Query.Where(e => e.Id >= start && e.Id <= end).ToList();


            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        #endregion
        #region GetById

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

        #endregion
        #region Exists

        [Theory]
        [TestJsonA("Exists", "", "A")]
        [TestJsonA("Exists", "", "B")]
        public void Exists(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = Repo.Exists(id);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJsonA("Exists", "", "A")]
        [TestJsonA("Exists", "", "B")]
        public async Task ExistsAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = await Repo.ExistsAsync(id);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        #endregion
        #region DynamicLinq

        [Theory]
        [TestJsonA("GetFromDynamicLinq", "WithSelect", "A")]
        public void GetFromDynamicLinqWithSelect(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            string orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            string select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            int? skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            int? take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<DynamicLinqResult,Rgb>("Expected");

            var actual = Repo.GetFromDynamicLinq(select, where, orderBy, skip, take);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, false));

        }


        [Theory]
        [TestJsonA("GetFromDynamicLinq", "WithSelect", "A")]
        public async Task GetFromDynamicLinqWithSelectAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            string orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            string select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            int? skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            int? take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<DynamicLinqResult, Rgb>("Expected");

            var actual = await Repo.GetFromDynamicLinqAsync(select, where, orderBy, skip, take);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, false));

        }

        [Theory]
        [TestJsonA("GetFromDynamicLinq", "WithoutSelect", "B")]
        public void GetFromDynamicLinqWithoutSelect(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            string orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            int? skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            int? take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<DynamicLinqResult<Rgb>>("Expected");

            var actual = Repo.GetFromDynamicLinq(where, orderBy, skip, take);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, false));

        }


        [Theory]
        [TestJsonA("GetFromDynamicLinq", "WithoutSelect", "B")]
        public async Task GetFromDynamicLinqWithoutSelectAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            string orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            int? skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            int? take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<DynamicLinqResult<Rgb>>("Expected");

            var actual = await Repo.GetFromDynamicLinqAsync(where, orderBy, skip, take);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, false));

        }


        #endregion


        [Theory]
        [TestJsonA("GetJsonFromJsonStoredProcedure", "", "A")]
        [TestJsonA("GetJsonFromJsonStoredProcedure", "", "B")]
        public void GetJsonFromJsonStoredProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };


            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<Rgb>(expectedJson);

            var actualJson = Repo.Context.GetJsonFromJsonStoredProcedure(spName, parameters);
            var actual = JsonSerializer.Deserialize<Rgb>(actualJson);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }



        [Theory]
        [TestJsonA("GetJsonFromJsonStoredProcedure", "", "A")]
        [TestJsonA("GetJsonFromJsonStoredProcedure", "", "B")]
        public async Task GetJsonFromJsonStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };


            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<Rgb>(expectedJson);

            var actualJson = await Repo.Context.GetJsonFromJsonStoredProcedureAsync(spName, parameters);
            var actual = JsonSerializer.Deserialize<Rgb>(actualJson);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }



    }

}
