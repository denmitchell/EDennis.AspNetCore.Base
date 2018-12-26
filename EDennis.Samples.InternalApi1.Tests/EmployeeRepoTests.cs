using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.InternalApi1.Models;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.InternalApi1.Tests {
    public class EmployeeRepoTests : InMemoryTest<HrContext> {

        private EmployeeRepo _repo;
        private readonly ITestOutputHelper _output;
        private string[] _propsToIgnore = new string[] { "SysStart", "SysEnd" };

        public EmployeeRepoTests(ITestOutputHelper output) {
            _repo = new EmployeeRepo(Context);
            _output = output;
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGet", testCase: "B")]
        public void TestCreateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = _repo.Create(input);
            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Create", testScenario: "CreateAndGetMultiple", testCase: "B")]
        public void TestCreateAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            var created = _repo.Create(input);
            var actual = _repo.GetByLinq(e => 1 == 1, 1, 1000);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public void TestDeleteAndGetMultipleEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Employee>>("Expected");

            _repo.Delete(id);
            var actual = _repo.GetByLinq(e => 1 == 1, 1, 1000);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }


        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "GetById", testScenario: "GetById", testCase: "B")]
        public void TestGetByIdEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = _repo.GetById(id);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
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

            var actual = _repo.GetByLinq(e=>e.FirstName.Contains(alpha),1,1000);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
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

            var actual = _repo.GetByLinq(e => e.FirstName.Contains(alpha), pageNumber, pageSize);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }

        [Theory]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "A")]
        [TestJson(className: "EmployeeRepo", methodName: "Update", testScenario: "UpdateAndGet", testCase: "B")]
        public void TestUpdateAndGetEmployee(string t, JsonTestCase jsonTestCase) {
            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Employee>("Input");
            var expected = jsonTestCase.GetObject<Employee>("Expected");

            var actual = _repo.Update(input);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
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
            var actual = _repo.GetByLinq(e => 1 == 1, 1, 1000);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }


    }
}
