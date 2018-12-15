using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.Samples.InternalApi1.Controllers;
using EDennis.Samples.InternalApi1.Models;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.InternalApi1.Tests {

    [Collection("Sequential")]
    public class EmployeeControllerUnitTests_InMemory : InMemoryUnitTest<HrContext> {

        private EmployeeController _controller;
        private EmployeeRepo _repo;
        private readonly ITestOutputHelper _output;

        public EmployeeControllerUnitTests_InMemory(ITestOutputHelper output) {
            _repo = new EmployeeRepo(Context);
            _controller = new EmployeeController(_repo);
            _output = output;
        }

        [Theory]
        [InlineData("Regis")]
        [InlineData("Wink")]
        [InlineData("Moe")]
        [InlineData("Larry")]
        [InlineData("Curly")]
        public void TestCreateEmployee(string firstName) {
            var max = Context.GetMaxKeyValue<Employee>();
            _output.WriteLine($"max of Employee Id: {max}");
            var response = _controller.CreateEmployee(new Employee { FirstName = firstName });
            var content = response.Result.GetObject<Employee>();
            var emp = _repo.GetByLinq(e => e.FirstName == firstName,1,1000)[0];
            _output.WriteLine($"Id: {emp.Id}, NamedInstance: {NamedInstance}");
            var count = _repo.GetByLinq(e => e.FirstName == firstName && e.Id == (max + 1), 1, 1000).Count;
            Assert.Equal(1, count);
        }


    }
}
