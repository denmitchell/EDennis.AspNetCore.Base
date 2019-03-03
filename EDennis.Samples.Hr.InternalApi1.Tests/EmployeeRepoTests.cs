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
            ConfigurationClassFixture<EmployeeRepo> fixture) : base(output, fixture) { }


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "B")]
        public void TestCreateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = Repo.Create(input);
            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "B")]
        public async Task TestCreateAndGetAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            await Repo.CreateAsync(input);
            var actual = Repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "B")]
        public void TestCreateAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            Repo.Create(input);
            var actual = Repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "B")]
        public async Task TestCreateAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var created = await Repo.CreateAsync(input);
            var actual = await Repo.Query.OrderBy(e=>e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public void TestDeleteAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            Repo.Delete(id);
            var actual = Repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public async Task TestDeleteAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            await Repo.DeleteAsync(id);
            var actual = await Repo.Query.OrderBy(e => e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }

        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "B")]
        public void TestGetByIdEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = Repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "B")]
        public async Task TestGetByIdAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = await Repo.GetByIdAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }

        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "B")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "C")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "D")]
        public void TestGetByLinqEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = Repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "B")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "C")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "D")]
        public async Task TestGetByLinqAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = await Repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }



        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "B")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "C")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "D")]
        public void TestGetByLinqPagingEmployee(string t, JsonTestCase jsonTestCase) {
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


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "B")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "C")]
        //[TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "D")]
        public async Task TestGetByLinqAsyncPagingEmployee(string t, JsonTestCase jsonTestCase) {
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



        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "B")]
        public void TestUpdateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            Repo.Update(input);
            var actual = Repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }



        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "B")]
        public async Task TestUpdateAndGetAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            Repo.Update(input);
            var actual = await Repo.GetByIdAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }

        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "B")]
        public void TestUpdateAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var updated = Repo.Update(input);
            var actual = Repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        //[Theory]
        //[TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "A")]
        //[TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "B")]
        public async Task TestUpdateAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var updated = await Repo.UpdateAsync(input);
            var actual = await Repo.Query.OrderBy(e => e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }





    }
}
