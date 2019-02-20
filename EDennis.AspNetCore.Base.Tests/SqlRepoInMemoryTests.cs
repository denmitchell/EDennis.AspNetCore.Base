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

    public class SqlRepoInMemoryTests : InMemoryRepoTests<ColorRepo, Color, ColorDbContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public SqlRepoInMemoryTests(ITestOutputHelper output, InMemoryClassFixture fixture)
            : base(output, fixture) { }


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
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            _repo.Create(input);
            var actual = _repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }



        [Theory]
        [TestJsonSpecific("Create", "SqlRepo", "brown")]
        [TestJsonSpecific("Create", "SqlRepo", "orange")]
        public async Task CreateAsync(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            await _repo.CreateAsync(input);
            var actual = _repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJsonSpecific("GetById", "SqlRepo", "1")]
        [TestJsonSpecific("GetById", "SqlRepo", "2")]
        public void GetById(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = _repo.GetById(input);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJsonSpecific("GetById", "SqlRepo", "1")]
        [TestJsonSpecific("GetById", "SqlRepo", "2")]
        public async Task GetByIdAsync(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = await _repo.GetByIdAsync(input);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJsonSpecific("Query", "SqlRepo", "A")]
        [TestJsonSpecific("Query", "SqlRepo", "B")]
        public void Query(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");

            var actual = _repo.Query.Where(c => c.Name.Contains(alpha)).ToPagedList(pageNumber, pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJsonSpecific("Query", "SqlRepo", "A")]
        [TestJsonSpecific("Query", "SqlRepo", "B")]
        public async Task QueryAsync(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");

            var actual = await _repo.Query.Where(c => c.Name.Contains(alpha)).ToPagedListAsync(pageNumber, pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJsonSpecific("Exists", "SqlRepo", "4")]
        [TestJsonSpecific("Exists", "SqlRepo", "999")]
        public void Exists(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = _repo.Exists(id);

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }

        [Theory]
        [TestJsonSpecific("Exists", "SqlRepo", "4")]
        [TestJsonSpecific("Exists", "SqlRepo", "999")]
        public async Task ExistsAsync(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = await _repo.ExistsAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }


        [Theory]
        [TestJson("ColorRepo", "Update", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        public void Update(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            _repo.Update(input);
            var actual = _repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }


        [Theory]
        [TestJson("ColorRepo", "Update", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        public async Task UpdateAsync(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            await _repo.UpdateAsync(input);
            var actual = _repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }


        [Theory]
        [TestJson("ColorRepo", "Delete", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            _repo.Delete(input);
            var actual = _repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }

        [Theory]
        [TestJson("ColorRepo", "Delete", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);

            await _repo.DeleteAsync(input);
            var actual = _repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }
    }
}
