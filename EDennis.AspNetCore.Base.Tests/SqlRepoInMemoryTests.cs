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

        public SqlRepoInMemoryTests(ITestOutputHelper output, ConfigurationClassFixture configFixture)
            : base(output, configFixture) { }


        [Theory]
        [TestJson("ColorRepo", "Create", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
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
        [TestJson("ColorRepo", "Create", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
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
        [TestJson("ColorRepo", "GetById", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        public void GetById(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = _repo.GetById(input);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJson("ColorRepo", "GetById", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        public async Task GetByIdAsync(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = await _repo.GetByIdAsync(input);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJson("ColorRepo", "GetByLinq", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        public void GetByLinq(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");

            var actual = _repo.Query.Where(c => c.Name.Contains(alpha)).ToPagedList(pageNumber, pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJson("ColorRepo", "GetByLinq", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        public async Task GetByLinqAsync(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");

            var actual = await _repo.Query.Where(c => c.Name.Contains(alpha)).ToPagedListAsync(pageNumber, pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJson("ColorRepo", "Exists", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        public void Exists(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = _repo.Exists(id);

            Assert.True(actual.IsEqualOrWrite(expected, _output));
        }

        [Theory]
        [TestJson("ColorRepo", "Exists", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
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
