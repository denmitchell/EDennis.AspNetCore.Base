using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.Samples.Hr.InternalApi1.Controllers;
using EDennis.Samples.Hr.InternalApi1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.InternalApi1.Tests {


    public class EmployeeControllerUnitTests_Clone :
        CloneRepoTests<EmployeeRepo, Employee, HrContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        private EmployeeController _controller;

        public EmployeeControllerUnitTests_Clone(ITestOutputHelper output,
            CloneClassFixture cloneFixture) : base(output, cloneFixture) {
            _controller = new EmployeeController(_repo);
        }



        [Theory]
        [InlineData("Regis")]
        [InlineData("Wink")]
        [InlineData("Moe")]
        [InlineData("Larry")]
        [InlineData("Curly")]
        public void TestCreateEmployee(string firstName) {

            _controller.CreateEmployee(new Employee { FirstName = firstName });

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



        [Theory]
        [InlineData(1, "Bob")]
        [InlineData(2, "Monty")]
        [InlineData(3, "Drew")]
        [InlineData(4, "Wayne")]
        public async Task TestGetEmployee(int id, string firstName) {

            var employee = (await _controller.GetEmployee(id)).GetObject();

            Assert.Equal(firstName, employee.FirstName);

        }

    }
}
