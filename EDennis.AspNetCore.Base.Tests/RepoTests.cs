using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Colors.InternalApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Tests {

    public class RepoTests : WriteableTemporalRepoTests<ColorRepo, Color, ColorDbContext, ColorHistoryDbContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public RepoTests(ITestOutputHelper output, 
            ConfigurationFactory<ColorRepo> fixture)
            : base(output, fixture, "moe@stooges.org") {
        }


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("ColorDb", "EDennis.Samples.Colors.InternalApi","ColorRepo", methodName, testScenario, testCase) {
            }
        }


        [Theory]
        [TestJson_("Create", "SqlRepo", "brown")]
        [TestJson_("Create", "SqlRepo", "orange")]
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
        [TestJson_("Create", "SqlRepo", "brown")]
        [TestJson_("Create", "SqlRepo", "orange")]
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
        [TestJson_("GetById", "SqlRepo", "1")]
        [TestJson_("GetById", "SqlRepo", "2")]
        public void GetById(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = Repo.GetById(input);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJson_("GetById", "SqlRepo", "1")]
        [TestJson_("GetById", "SqlRepo", "2")]
        public async Task GetByIdAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = await Repo.GetByIdAsync(input);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("GetByIdAsOf", "SqlRepo", "1")]
        [TestJson_("GetByIdAsOf", "SqlRepo", "3")]
        public void GetByIdAsOf(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var asOf = jsonTestCase.GetObject<DateTime>("AsOf");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Color>("Expected");

            var actual = Repo.GetByIdAsOf(asOf,id);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("GetByIdHistory", "SqlRepo", "1")]
        [TestJson_("GetByIdHistory", "SqlRepo", "3")]
        public void GetByIdHistory(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x=>x.Id)
                .ThenByDescending(x=>x.SysStart);

            var actual = Repo.GetByIdHistory(id)
                .OrderBy(x => x.Id)
                .ThenByDescending(x => x.SysStart);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("QueryAsOf", "SqlRepo", "A")]
        [TestJson_("QueryAsOf", "SqlRepo", "B")]
        public void QueryAsOf(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var asOf = jsonTestCase.GetObject<DateTime>("AsOf");
            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id)
                .ThenByDescending(x => x.SysStart);

            var actual = Repo.QueryAsOf(asOf,
                x=>x.Name.Contains(alpha),
                1,1000,
                asc=>asc.Id,desc=>desc.SysStart
                )
                .OrderBy(x => x.Id)
                .ThenByDescending(x => x.SysStart);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("QueryAsOfRange", "SqlRepo", "A")]
        [TestJson_("QueryAsOfRange", "SqlRepo", "B")]
        public void QueryAsOfRange(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var from = jsonTestCase.GetObject<DateTime>("From");
            var to = jsonTestCase.GetObject<DateTime>("To");
            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id)
                .ThenByDescending(x => x.SysStart);

            var actual = Repo.QueryAsOf(from,to,
                x => x.Name.Contains(alpha),
                1, 1000,
                asc => asc.Id, desc => desc.SysStart
                )
                .OrderBy(x => x.Id)
                .ThenByDescending(x => x.SysStart);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Query", "SqlRepo", "A")]
        [TestJson_("Query", "SqlRepo", "B")]
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
        [TestJson_("Query", "SqlRepo", "A")]
        [TestJson_("Query", "SqlRepo", "B")]
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
        [TestJson_("Exists", "SqlRepo", "4")]
        [TestJson_("Exists", "SqlRepo", "999")]
        public void Exists(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = Repo.Exists(id);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

        [Theory]
        [TestJson_("Exists", "SqlRepo", "4")]
        [TestJson_("Exists", "SqlRepo", "999")]
        public async Task ExistsAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<bool>("Expected");

            var actual = await Repo.ExistsAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }


        [Theory]
        [TestJson_("Update", "SqlRepo", "1")]
        [TestJson_("Update", "SqlRepo", "2")]
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

            /*
            var wholeHistory = Repo.QueryAsOf(
                new System.DateTime(2015, 7, 1),
                new System.DateTime(2017, 7, 1),
                x => x.Name.Contains("e"),
                1, 10000,
                asc=>asc.Id, desc=>desc.SysStart).ToList();

            */
            var actualHistory = Repo.GetByIdHistory(id)
                .Skip(1)
                .OrderByDescending(x => x.SysStart);


            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
            Assert.True(actualHistory.IsEqualOrWrite(expectedHistory, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Update", "SqlRepo", "1")]
        [TestJson_("Update", "SqlRepo", "2")]
        public async Task UpdateAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);
            var expectedHistory = jsonTestCase.GetObject<List<Color>>("ExpectedHistory")
                .Skip(1)
                .OrderByDescending(x => x.SysStart);

            await Repo.UpdateAsync(input,id);
            var actual = Repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Delete", "SqlRepo", "3")]
        [TestJson_("Delete", "SqlRepo", "4")]
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
        [TestJson_("Delete", "SqlRepo", "3")]
        [TestJson_("Delete", "SqlRepo", "4")]
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
