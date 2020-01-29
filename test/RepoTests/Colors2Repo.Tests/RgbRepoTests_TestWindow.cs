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

namespace Colors2Repo.Tests {

    [Collection("Repo Tests")]
    public class RgbRepoTests_TestWindow
        : RepoTests<RgbRepo, Rgb, Color2DbContext> {

        public RgbRepoTests_TestWindow(ITestOutputHelper output)
            : base(output) { }

        protected string[] PropertiesToIgnore { get; }
            = new string[] { "SysStart", "SysEnd" };


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
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
        [TestJson_("Create", "", "A")]
        [TestJson_("Create", "", "B")]
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
        [TestJson_("Create", "", "A")]
        [TestJson_("Create", "", "B")]
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
        [TestJson_("Update", "", "A")]
        [TestJson_("Update", "", "B")]
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
        [TestJson_("Update", "", "A")]
        [TestJson_("Update", "", "B")]
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
        [TestJson_("Delete", "", "A")]
        [TestJson_("Delete", "", "B")]
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
        [TestJson_("Delete", "", "A")]
        [TestJson_("Delete", "", "B")]
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
        #region Get

        [Theory]
        [TestJson_("Get", "", "A")]
        [TestJson_("Get", "", "B")]
        public void Get(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Rgb>("Expected");

            var actual = Repo.Get(id);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJson_("Get", "", "A")]
        [TestJson_("Get", "", "B")]
        public async Task GetAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Rgb>("Expected");

            var actual = await Repo.GetAsync(id);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        #endregion
        #region Exists

        [Theory]
        [TestJson_("Exists", "", "A")]
        [TestJson_("Exists", "", "B")]
        public void Exists(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = Repo.Exists(id);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJson_("Exists", "", "A")]
        [TestJson_("Exists", "", "B")]
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
        [TestJson_("GetWithDynamicLinq", "WithSelect", "A")]
        public void GetWithDynamicLinqWithSelect(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            string orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            string select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            int? skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            int? take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<DynamicLinqResult,Rgb>("Expected");

            var actual = Repo.GetWithDynamicLinq(select, where, orderBy, skip, take);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, false));

        }


        [Theory]
        [TestJson_("GetWithDynamicLinq", "WithSelect", "A")]
        public async Task GetWithDynamicLinqWithSelectAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            string orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            string select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            int? skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            int? take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<DynamicLinqResult, Rgb>("Expected");

            var actual = await Repo.GetWithDynamicLinqAsync(select, where, orderBy, skip, take);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, false));

        }

        [Theory]
        [TestJson_("GetWithDynamicLinq", "WithoutSelect", "B")]
        public void GetWithDynamicLinqWithoutSelect(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            string orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            int? skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            int? take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<DynamicLinqResult<Rgb>>("Expected");

            var actual = Repo.GetWithDynamicLinq(where, orderBy, skip, take);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, false));

        }


        [Theory]
        [TestJson_("GetWithDynamicLinq", "WithoutSelect", "B")]
        public async Task GetWithDynamicLinqWithoutSelectAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            string where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            string orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            int? skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            int? take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<DynamicLinqResult<Rgb>>("Expected");

            var actual = await Repo.GetWithDynamicLinqAsync(where, orderBy, skip, take);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, false));

        }


        #endregion
        #region GetJsonArrayFromStoredProcedure

        [Theory]
        [TestJson_("GetJsonArrayFromStoredProcedure", "", "A")]
        public void GetJsonArrayFromStoredProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorNameContains = jsonTestCase.GetObject<string>("ColorNameContains");
            var parameters = new Dictionary<string, object> { { "ColorNameContains", colorNameContains } };

            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<List<dynamic>>(expectedJson, DynamicJsonSerializerOptions);

            var actualJson = Repo.Context.GetJsonArrayFromStoredProcedure(spName, parameters);
            var actual = JsonSerializer.Deserialize<List<dynamic>>(actualJson, DynamicJsonSerializerOptions);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJson_("GetJsonArrayFromStoredProcedure", "", "A")]
        public async Task GetJsonArrayFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorNameContains = jsonTestCase.GetObject<string>("ColorNameContains");
            var parameters = new Dictionary<string, object> { { "ColorNameContains", colorNameContains } };

            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<List<dynamic>>(expectedJson, DynamicJsonSerializerOptions);

            var actualJson = await Repo.Context.GetJsonArrayFromStoredProcedureAsync(spName, parameters);
            var actual = JsonSerializer.Deserialize<List<dynamic>>(actualJson, DynamicJsonSerializerOptions);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        #endregion
        #region GetJsonObjectFromStoredProcedure

        [Theory]
        [TestJson_("GetJsonObjectFromStoredProcedure", "", "A")]
        public void GetJsonObjectFromStoredProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<dynamic>(expectedJson, DynamicJsonSerializerOptions);

            var actualJson = Repo.Context.GetJsonObjectFromStoredProcedure(spName, parameters);
            var actual = JsonSerializer.Deserialize<dynamic>(actualJson, DynamicJsonSerializerOptions);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, true));
        }

        [Theory]
        [TestJson_("GetJsonObjectFromStoredProcedure", "", "A")]
        public async Task GetJsonObjectFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<dynamic>(expectedJson, DynamicJsonSerializerOptions);

            var actualJson = await Repo.Context.GetJsonObjectFromStoredProcedureAsync(spName, parameters);
            var actual = JsonSerializer.Deserialize<dynamic>(actualJson, DynamicJsonSerializerOptions);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual,expected, 3, PropertiesToIgnore, Output, true));
        }

        #endregion
        #region GetJsonFromJsonStoredProcedure

        [Theory]
        [TestJson_("GetJsonFromJsonStoredProcedure", "", "A")]
        [TestJson_("GetJsonFromJsonStoredProcedure", "", "B")]
        public void GetJsonFromJsonStoredProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<dynamic>(expectedJson, DynamicJsonSerializerOptions);

            var actualJson = Repo.Context.GetJsonFromJsonStoredProcedure(spName, parameters);
            var actual = JsonSerializer.Deserialize<dynamic>(actualJson, DynamicJsonSerializerOptions);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual,expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJson_("GetJsonFromJsonStoredProcedure", "", "A")]
        [TestJson_("GetJsonFromJsonStoredProcedure", "", "B")]
        public async Task GetJsonFromJsonStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expectedJson = jsonTestCase.GetObject<string>("Expected");
            var expected = JsonSerializer.Deserialize<dynamic>(expectedJson, DynamicJsonSerializerOptions);

            var actualJson = await Repo.Context.GetJsonFromJsonStoredProcedureAsync(spName, parameters);
            var actual = JsonSerializer.Deserialize<dynamic>(actualJson, DynamicJsonSerializerOptions);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, true));
        }

        #endregion
        #region GetListFromStoredProcedure

        [Theory]
        [TestJson_("GetListFromStoredProcedure", "", "A")]
        public void GetListFromStoredProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorNameContains = jsonTestCase.GetObject<string>("ColorNameContains");
            var parameters = new Dictionary<string, object> { { "ColorNameContains", colorNameContains } };

            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var actual = Repo.Context.GetListFromStoredProcedure<Color2DbContext,Rgb>(spName, parameters);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }


        [Theory]
        [TestJson_("GetListFromStoredProcedure", "", "A")]
        public async Task GetListFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorNameContains = jsonTestCase.GetObject<string>("ColorNameContains");
            var parameters = new Dictionary<string, object> { { "ColorNameContains", colorNameContains } };

            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");
            var actual = await Repo.Context.GetListFromStoredProcedureAsync<Color2DbContext, Rgb>(spName, parameters);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
        }

        #endregion
        #region GetObjectFromStoredProcedure

        [Theory]
        [TestJson_("GetObjectFromStoredProcedure", "", "A")]
        public void GetObjectFromStoredProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expected = jsonTestCase.GetObject<Rgb>("Expected");
            var actual = Repo.Context.GetObjectFromStoredProcedure<Color2DbContext, Rgb>(spName, parameters);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, true));
        }

        [Theory]
        [TestJson_("GetObjectFromStoredProcedure", "", "A")]
        public async Task GetObjectFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expected = jsonTestCase.GetObject<Rgb>("Expected");
            var actual = await Repo.Context.GetObjectFromStoredProcedureAsync<Color2DbContext, Rgb>(spName, parameters);

            Assert.True(ObjectExtensions.IsEqualAndWrite(actual, expected, 3, PropertiesToIgnore, Output, true));
        }

        #endregion


        #region GetScalarFromStoredProcedure

        [Theory]
        [TestJson_("GetScalarFromStoredProcedure", "IntResult", "B")]
        public void GetScalarIntFromStoredProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expected = jsonTestCase.GetObject<int>("Expected");
            var actual = Repo.Context.GetScalarFromStoredProcedure<Color2DbContext,int>(spName, parameters);

            Assert.Equal(expected,actual);
        }

        [Theory]
        [TestJson_("GetScalarFromStoredProcedure", "StringResult", "A")]
        public void GetScalarStringFromStoredProcedure(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expected = jsonTestCase.GetObject<string>("Expected");
            var actual = Repo.Context.GetScalarFromStoredProcedure<Color2DbContext, string>(spName, parameters);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [TestJson_("GetScalarFromStoredProcedure", "IntResult", "B")]
        public async Task GetScalarIntFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expected = jsonTestCase.GetObject<int>("Expected");
            var actual = await Repo.Context.GetScalarFromStoredProcedureAsync<Color2DbContext, int>(spName, parameters);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [TestJson_("GetScalarFromStoredProcedure", "StringResult", "A")]
        public async Task GetScalarStringFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            var spName = jsonTestCase.GetObject<string>("SpName");
            var colorName = jsonTestCase.GetObject<string>("ColorName");
            var parameters = new Dictionary<string, object> { { "ColorName", colorName } };

            var expected = jsonTestCase.GetObject<string>("Expected");
            var actual = await Repo.Context.GetScalarFromStoredProcedureAsync<Color2DbContext, string>(spName, parameters);

            Assert.Equal(expected, actual);
        }


        #endregion



    }

}
