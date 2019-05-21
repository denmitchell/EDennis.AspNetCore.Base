using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Hr.InternalApi1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.InternalApi1.Tests {
    public class EmployeeRepoTests :
        WriteableTemporalRepoTests<EmployeeRepo, Employee, HrContext, HrHistoryContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public EmployeeRepoTests(ITestOutputHelper output,
            ConfigurationFactory<EmployeeRepo> fixture) : base(output, fixture) {
        }

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Hr", "EDennis.Samples.Hr.InternalApi1", "EmployeeRepo", methodName, testScenario, testCase) {
            }
        }

        [Theory]
        [TestJson_("Create", "CreateAndGet", "Larry")]
        [TestJson_("Create", "CreateAndGet", "Curly")]
        public void CreateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = Repo.Create(input);
            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Create", "CreateAndGet", "Larry")]
        [TestJson_("Create", "CreateAndGet", "Curly")]
        public async Task CreateAndGetAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            await Repo.CreateAsync(input);
            var actual = Repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Create", "CreateAndGetMultiple", "Larry")]
        [TestJson_("Create", "CreateAndGetMultiple", "Curly")]
        public void CreateAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            Repo.Create(input);
            var actual = Repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Create", "CreateAndGetMultiple", "Larry")]
        [TestJson_("Create", "CreateAndGetMultiple", "Curly")]
        public async Task CreateAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var created = await Repo.CreateAsync(input);
            var actual = await Repo.Query.OrderBy(e=>e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Delete", "DeleteAndGetMultiple", "1")]
        [TestJson_("Delete", "DeleteAndGetMultiple", "2")]
        public void DeleteAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            Repo.Delete(id);
            var actual = Repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Delete", "DeleteAndGetMultiple", "1")]
        [TestJson_("Delete", "DeleteAndGetMultiple", "2")]
        public async Task DeleteAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            await Repo.DeleteAsync(id);
            var actual = await Repo.Query.OrderBy(e => e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJson_("GetById", "GetById", "1")]
        [TestJson_("GetById", "GetById", "2")]
        public void GetByIdEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = Repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("GetById", "GetById", "1")]
        [TestJson_("GetById", "GetById", "2")]
        public async Task GetByIdAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = await Repo.GetByIdAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJson_("Query", "Query", "A")]
        [TestJson_("Query", "Query", "B")]
        [TestJson_("Query", "Query", "C")]
        [TestJson_("Query", "Query", "D")]
        public void QueryEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = Repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Query", "Query", "A")]
        [TestJson_("Query", "Query", "B")]
        [TestJson_("Query", "Query", "C")]
        [TestJson_("Query", "Query", "D")]
        public async Task QueryAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = await Repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }



        [Theory]
        [TestJson_("Query", "PagedList", "A")]
        [TestJson_("Query", "PagedList", "B")]
        [TestJson_("Query", "PagedList", "C")]
        [TestJson_("Query", "PagedList", "D")]
        public void PagedListEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = Repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToPagedList(pageNumber,pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Query", "PagedList", "A")]
        [TestJson_("Query", "PagedList", "B")]
        [TestJson_("Query", "PagedList", "C")]
        [TestJson_("Query", "PagedList", "D")]
        public async Task PagedListAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = await Repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToPagedListAsync(pageNumber, pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }



        [Theory]
        [TestJson_("Update","UpdateAndGet","A")]
        [TestJson_("Update","UpdateAndGet","B")]
        public void UpdateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            Repo.Update(input,id);
            var actual = Repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }



        [Theory]
        [TestJson_("Update","UpdateAndGet","A")]
        [TestJson_("Update","UpdateAndGet","B")]
        public async Task UpdateAndGetAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            Repo.Update(input,id);
            var actual = await Repo.GetByIdAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJson_("Update", "UpdateAndGetMultiple","A")]
        [TestJson_("Update", "UpdateAndGetMultiple","B")]
        public void UpdateAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var updated = Repo.Update(input,id);
            var actual = Repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Update", "UpdateAndGetMultiplePatch", "A")]
        [TestJson_("Update", "UpdateAndGetMultiplePatch", "B")]
        public void UpdateAndGetMultipleEmployeePatch(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            dynamic input = jsonTestCase.GetObject<dynamic>("Input");

            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var updated = Repo.Update(input, id);
            var actual = Repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }



        [Theory]
        [TestJson_("Update","UpdateAndGetMultiple","A")]
        [TestJson_("Update","UpdateAndGetMultiple","B")]
        public async Task UpdateAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var updated = await Repo.UpdateAsync(input,id);
            var actual = await Repo.Query.OrderBy(e => e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }





    }
}
