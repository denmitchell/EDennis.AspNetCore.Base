using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Hr.InternalApi1.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
            ConfigurationClassFixture<EmployeeRepo> fixture) : base(output, fixture) {
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "Larry")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "Curly")]
        public void CreateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = Repo.Create(input);
            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "Larry")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "Curly")]
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
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "Larry")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "Curly")]
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
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "Larry")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "Curly")]
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
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "1")]
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "2")]
        public void DeleteAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            Repo.Delete(id);
            var actual = Repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "1")]
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "2")]
        public async Task DeleteAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            await Repo.DeleteAsync(id);
            var actual = await Repo.Query.OrderBy(e => e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "1")]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "2")]
        public void GetByIdEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = Repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "1")]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "2")]
        public async Task GetByIdAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = await Repo.GetByIdAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "Query", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "Query", testCase: "B")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "Query", testCase: "C")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "Query", testCase: "D")]
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
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "Query", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "Query", testCase: "B")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "Query", testCase: "C")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "Query", testCase: "D")]
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
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "PagedList", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "PagedList", testCase: "B")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "PagedList", testCase: "C")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "PagedList", testCase: "D")]
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
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "PagedList", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "PagedList", testCase: "B")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "PagedList", testCase: "C")]
        [TestJson(className: "EmployeeRepo", methodName: "Query", testScenario: "PagedList", testCase: "D")]
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
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "B")]
        public void TestUpdateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            Repo.Update(input,id);
            var actual = Repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }



        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "B")]
        public async Task TestUpdateAndGetAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            Repo.Update(input,id);
            var actual = await Repo.GetByIdAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "B")]
        public void TestUpdateAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var updated = Repo.Update(input,id);
            var actual = Repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "B")]
        public async Task TestUpdateAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
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
