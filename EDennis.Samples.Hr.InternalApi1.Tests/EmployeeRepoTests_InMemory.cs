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
    public class EmployeeRepoTests_InMemory :
        InMemoryRepoTests<EmployeeRepo, Employee, HrContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public EmployeeRepoTests_InMemory(ITestOutputHelper output,
            ConfigurationClassFixture configFixture)
            : base(output, configFixture) { }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "B")]
        public void TestCreateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = _repo.Create(input);
            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "B")]
        public async Task TestCreateAndGetAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            await _repo.CreateAsync(input);
            var actual = _repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "B")]
        public void TestCreateAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            _repo.Create(input);
            var actual = _repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "B")]
        public async Task TestCreateAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var created = await _repo.CreateAsync(input);
            var actual = await _repo.Query.OrderBy(e=>e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public void TestDeleteAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            _repo.Delete(id);
            var actual = _repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public async Task TestDeleteAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            await _repo.DeleteAsync(id);
            var actual = await _repo.Query.OrderBy(e => e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "B")]
        public void TestGetByIdEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = _repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "B")]
        public async Task TestGetByIdAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = await _repo.GetByIdAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "B")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "C")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "D")]
        public void TestGetByLinqEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = _repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "B")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "C")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinq", testCase: "D")]
        public async Task TestGetByLinqAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = await _repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }



        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "B")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "C")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "D")]
        public void TestGetByLinqPagingEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = _repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToPagedList(pageNumber,pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "B")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "C")]
        [TestJson(className: "EmployeeRepo", methodName: "GetByLinq", testScenario: "GetByLinqPaging", testCase: "D")]
        public async Task TestGetByLinqAsyncPagingEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var alpha = jsonTestCase.GetObject<string>("Alpha");
            var pageNumber = jsonTestCase.GetObject<int>("PageNumber");
            var pageSize = jsonTestCase.GetObject<int>("PageSize");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var actual = await _repo.Query
                .Where(e => e.FirstName.Contains(alpha))
                .OrderBy(e => e.Id)
                .ToPagedListAsync(pageNumber, pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetFromSql", testScenario: "GetFromSql", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "GetFromSql", testScenario: "GetFromSql", testCase: "B")]
        public void TestGetFromSqlEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var firstName = jsonTestCase.GetObject<string>("FirstName");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var sql = "select * from Employee where FirstName = {0}";
            var actual = _repo.GetFromSql(sql, 1, 1000, true, firstName);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));

        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetFromSql", testScenario: "GetFromSql", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "GetFromSql", testScenario: "GetFromSql", testCase: "B")]
        public async Task TestGetFromSqlAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var firstName = jsonTestCase.GetObject<string>("FirstName");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var sql = "select * from Employee where FirstName = {0}";
            var actual = await _repo.GetFromSqlAsync(sql, 1, 1000, true, firstName);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));

        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "B")]
        public void TestUpdateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            _repo.Update(input);
            var actual = _repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }



        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "B")]
        public async Task TestUpdateAndGetAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            _repo.Update(input);
            var actual = await _repo.GetByIdAsync(id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "B")]
        public void TestUpdateAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var updated = _repo.Update(input);
            var actual = _repo.Query.OrderBy(e => e.Id).ToList();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGetMultiple", testCase: "B")]
        public async Task TestUpdateAndGetMultipleAsyncEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var updated = await _repo.UpdateAsync(input);
            var actual = await _repo.Query.OrderBy(e => e.Id).ToListAsync();

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, _output));
        }




        [Theory]
        [InlineData("Regis")]
        [InlineData("Wink")]
        [InlineData("Moe")]
        [InlineData("Larry")]
        [InlineData("Curly")]
        public void TestCreateEmployee(string firstName) {

            var preCount = _repo.GetScalarFromDapper<int>("select count(*) recs from employee");

            _repo.Create(new Employee { FirstName = firstName });

            var postCount = _repo.GetScalarFromDapper<int>("select count(*) recs from employee");

            Assert.Equal(preCount + 1, postCount);

            var employees = _repo.Query.ToList()
                .OrderBy(e => e.Id);

            Assert.Collection(employees,
                new Action<Employee>[] {
                    item=>Assert.Contains("Bob",item.FirstName),
                    item=>Assert.Contains("Monty",item.FirstName),
                    item=>Assert.Contains("Drew",item.FirstName),
                    item=>Assert.Contains("Wayne",item.FirstName),
                    item=>Assert.Contains(firstName,item.FirstName),
                });
        }

    }
}
