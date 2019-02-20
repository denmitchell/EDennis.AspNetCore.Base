using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Colors.InternalApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class SqlRepoCloneTests : CloneRepoTests<ColorRepo,Color,ColorDbContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public SqlRepoCloneTests(ITestOutputHelper output, CloneClassFixture<ColorDbContext> cloneFixture)
            : base(output, cloneFixture) { }


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


        //[Theory]
        //[InlineData("A")]
        //[InlineData("B")]
        //[InlineData("C")]
        //[InlineData("D")]
        //[InlineData("E")]
        //[InlineData("F")]
        //[InlineData("G")]
        //[InlineData("H")]
        //[InlineData("I")]
        //[InlineData("J")]
        //[InlineData("K")]
        //[InlineData("L")]
        //[InlineData("M")]
        //public void Create(string testCase) {
        //    _repo.Create(new Color { Name = testCase });
        //}


        //[Theory]
        //[InlineData("A")]
        //[InlineData("B")]
        //[InlineData("C")]
        //[InlineData("D")]
        //[InlineData("E")]
        //[InlineData("F")]
        //[InlineData("G")]
        //[InlineData("H")]
        //[InlineData("I")]
        //[InlineData("J")]
        //[InlineData("K")]
        //[InlineData("L")]
        //[InlineData("M")]
        //public void Create2(string testCase) {
        //    _repo.Create(new Color { Name = testCase });
        //}


        //[Theory]
        //[TestJson("ColorRepo", "DeleteUpdate", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        //public void DeleteUpdate(string t, JsonTestCase jsonTestCase) {
        //    _output.WriteLine(t);

        //    var input = jsonTestCase.GetObject<Color>("Input");
        //    var expected = jsonTestCase.GetObject<List<Color>>("Expected")
        //        .OrderBy(x => x.Id);
        //    var expectedHistory = jsonTestCase.GetObject<List<Color>>("ExpectedHistory")
        //        .OrderBy(x => x.Id);

        //    _repo.DeleteUpdate(input);

        //    var actual = _repo.GetByLinq(x => true)
        //        .OrderBy(x => x.Id);

        //    var actualHistory = _repo.GetFromSql("select * from dbo_history.Colors")
        //        .OrderBy(x => x.Id);

        //    Assert.True(actual.IsEqualOrWrite(expected, _output));
        //    Assert.True(actualHistory.IsEqualOrWrite(expectedHistory, _output));
        //}


        //[Theory]
        //[TestJson("ColorRepo", "DeleteUpdate", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        //public async Task DeleteUpdateAsync(string t, JsonTestCase jsonTestCase) {
        //    _output.WriteLine(t);

        //    var input = jsonTestCase.GetObject<Color>("Input");
        //    var expected = jsonTestCase.GetObject<List<Color>>("Expected")
        //        .OrderBy(x => x.Id);
        //    var expectedHistory = jsonTestCase.GetObject<List<Color>>("ExpectedHistory")
        //        .OrderBy(x => x.Id);

        //    await _repo.DeleteUpdateAsync(input);

        //    var actual = _repo.GetByLinq(x => true)
        //        .OrderBy(x => x.Id);

        //    var actualHistory = _repo.GetFromSql("select * from dbo_history.Colors")
        //        .OrderBy(x => x.Id);

        //    Assert.True(actual.IsEqualOrWrite(expected, _output));
        //    Assert.True(actualHistory.IsEqualOrWrite(expectedHistory, _output));
        //}


        //[Theory]
        //[TestJson("ColorRepo", "GetFromSql", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        //public void GetFromSql(string t, JsonTestCase jsonTestCase) {
        //    _output.WriteLine(t);

        //    var alpha = jsonTestCase.GetObject<string>("Alpha");
        //    var id = jsonTestCase.GetObject<int>("Id");
        //    var expected = jsonTestCase.GetObject<List<Color>>("Expected")
        //        .OrderByDescending(x => x.SysStart);

        //    var sql = $@"
        //        select * 
        //            from Colors 
        //                where Name like '{alpha}%' 
        //        except 
        //        select * 
        //            from dbo_history.Colors 
        //                where Id = {id}";

        //    var actual = _repo.GetFromSql(sql)
        //        .OrderBy(x => x.Id);

        //    _output.WriteLine(actual.ToJsonString());

        //    Assert.True(actual.IsEqualOrWrite(expected, _output));
        //}


        //[Theory]
        //[TestJson("ColorRepo", "GetByIdAsOf", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        //public void GetByIdAsOf(string t, JsonTestCase jsonTestCase) {
        //    _output.WriteLine(t);

        //    var id = jsonTestCase.GetObject<int>("Id");
        //    var input1 = jsonTestCase.GetObject<Color>("UpdateInput1");
        //    var input2 = jsonTestCase.GetObject<Color>("UpdateInput2");
        //    var expected = jsonTestCase.GetObject<Color>("Expected");

        //    _repo.Update(input1);
        //    Thread.Sleep(1000);
        //    _repo.Update(input2);

        //    var hist = _repo.GetFromDapper<Color>("select Id, Name, SysStart, SysEnd from dbo_history.colors");

        //    var asOf = hist
        //            .OrderBy(x => x.SysStart)
        //            .Skip(1)
        //            .Take(1)
        //            .FirstOrDefault()
        //            .SysStart
        //            .AddMilliseconds(-25);


        //    //var sql = $@"
        //    //    select asOf = dateadd(MILLISECOND,25,max(SysStart))
        //    //     from
        //    //     (select row_number() over (partition by Id order by SysStart) rowid, SysStart 
        //    //      from dbo_history.Colors
        //    //      where Id = {id}) r
        //    //     where rowid = 2;		
        //    //    ";

        //    //var asOf = _repo.GetScalarFromDapper<DateTime>(sql);

        //    var actual = _repo.GetByIdAsOf(asOf, id);

        //    _output.WriteLine(actual.ToJsonString());

        //    Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        //}



        //[Theory]
        //[TestJson("ColorRepo", "GetByIdHistory", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        //public void GetByIdHistory(string t, JsonTestCase jsonTestCase) {
        //    _output.WriteLine(t);

        //    var id = jsonTestCase.GetObject<int>("Id");
        //    var input1 = jsonTestCase.GetObject<Color>("UpdateInput1");
        //    var input2 = jsonTestCase.GetObject<Color>("UpdateInput2");
        //    var expected = jsonTestCase.GetObject<List<Color>>("Expected")
        //        .OrderByDescending(c => c.SysStart);

        //    _repo.Update(input1);
        //    _repo.Update(input2);

        //    var actual = _repo.GetByIdHistory(id)
        //        .OrderByDescending(c => c.SysStart);

        //    _output.WriteLine(actual.ToJsonString());

        //    Assert.True(actual.IsEqualOrWrite(expected, _output));
        //}




        //[Theory]
        //[TestJson("ColorRepo", "GetFromJsonSql", "SqlRepo", "A", "TestJsonConfigs\\InternalApi.json")]
        //public void GetFromJsonSql(string t, JsonTestCase jsonTestCase) {
        //    _output.WriteLine(t);

        //    var alpha = jsonTestCase.GetObject<string>("Alpha");
        //    var expected = jsonTestCase.JsonTestFiles
        //        .Where(f => f.TestFile == "Expected").FirstOrDefault().Json; //get raw json

        //    var sql = $@"
        //     select * 
        //      from Colors 
        //      where Name Like '%{alpha}%' 
        //     for json path, include_null_values
        //        ";

        //    var actual = _repo.GetFromJsonSql(sql);

        //    _output.WriteLine(actual);

        //    Assert.True(actual.IsEqualOrWrite(expected, _output));
        //}

    }
}
