using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Colors.InternalApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class RepoTests : WriteableTemporalRepoTests<ColorRepo, Color, ColorDbContext, ColorHistoryDbContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public RepoTests(ITestOutputHelper output, ConfigurationClassFixture fixture)
            : base(output, fixture) {

            ScopeProperties.User = "moe@stooges.org";
        }


        /// <summary>
        /// Optional internal class ... reduced the number of parameters in TestJson attribute
        /// by specifying constant parameter values for className and testJsonConfigPath here
        /// </summary>
        internal class TestJsonSpecific : TestJsonAttribute {
            public TestJsonSpecific(string methodName, string testScenario, string testCase)
                : base("ColorRepo", methodName, testScenario, testCase, "TestJsonConfigs\\InternalApi.json") {
            }
        }


        [Theory]
        [TestJsonSpecific("Create", "SqlRepo", "brown")]
        [TestJsonSpecific("Create", "SqlRepo", "orange")]
        public void Create(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            Repo.Create(input);
            var actual = Repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }



        [Theory]
        [TestJsonSpecific("Create", "SqlRepo", "brown")]
        [TestJsonSpecific("Create", "SqlRepo", "orange")]
        public async Task CreateAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            await Repo.CreateAsync(input);
            var actual = Repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJsonSpecific("GetById", "SqlRepo", "1")]
        [TestJsonSpecific("GetById", "SqlRepo", "2")]
        public void GetById(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = Repo.GetById(input);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJsonSpecific("GetById", "SqlRepo", "1")]
        [TestJsonSpecific("GetById", "SqlRepo", "2")]
        public async Task GetByIdAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = await Repo.GetByIdAsync(input);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJsonSpecific("Query", "SqlRepo", "A")]
        [TestJsonSpecific("Query", "SqlRepo", "B")]
        public void Query(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");

            var actual = Repo.Query.Where(c => c.Name.Contains(alpha)).ToPagedList(pageNumber, pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJsonSpecific("Query", "SqlRepo", "A")]
        [TestJsonSpecific("Query", "SqlRepo", "B")]
        public async Task QueryAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");

            var actual = await Repo.Query.Where(c => c.Name.Contains(alpha)).ToPagedListAsync(pageNumber, pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJsonSpecific("Exists", "SqlRepo", "4")]
        [TestJsonSpecific("Exists", "SqlRepo", "999")]
        public void Exists(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = Repo.Exists(id);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

        [Theory]
        [TestJsonSpecific("Exists", "SqlRepo", "4")]
        [TestJsonSpecific("Exists", "SqlRepo", "999")]
        public async Task ExistsAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = await Repo.ExistsAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }


        [Theory]
        [TestJsonSpecific("Update", "SqlRepo", "1")]
        [TestJsonSpecific("Update", "SqlRepo", "2")]
        public void Update(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);
            var expectedHistory = jsonTestCase.GetObject<List<Color>>("ExpectedHistory")
                .OrderByDescending(x => x.SysStart);

            Repo.Update(input,id);
            var actual = Repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            var actualHistory = Repo.GetByIdHistory(id)
                .OrderByDescending(x => x.SysStart);


            Assert.True(actual.IsEqualOrWrite(expected, Output));
            Assert.True(actualHistory.IsEqualOrWrite(expectedHistory, Output));
        }


        [Theory]
        [TestJsonSpecific("Update", "SqlRepo", "1")]
        [TestJsonSpecific("Update", "SqlRepo", "2")]
        public async Task UpdateAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            await Repo.UpdateAsync(input,id);
            var actual = Repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }


        [Theory]
        [TestJsonSpecific("Delete", "SqlRepo", "3")]
        [TestJsonSpecific("Delete", "SqlRepo", "4")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            Repo.Delete(input);
            var actual = Repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

        [Theory]
        [TestJsonSpecific("Delete", "SqlRepo", "3")]
        [TestJsonSpecific("Delete", "SqlRepo", "4")]
        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            await Repo.DeleteAsync(input);
            var actual = Repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }
    }
}
