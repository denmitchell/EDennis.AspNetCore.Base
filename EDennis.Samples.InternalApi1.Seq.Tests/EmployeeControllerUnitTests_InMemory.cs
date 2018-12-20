using EDennis.NetCoreTestingUtilities;
using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.InternalApi1.Models;
using EDennis.Samples.InternalApi1.Seq.Controllers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.InternalApi1.Tests {
    public class EmployeeControllerUnitTests_InMemory : InMemoryTest<HrContext> {

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
        public async Task TestCreateEmployee(string firstName) {
            var max = Context.GetMaxKeyValue<Employee>();
            _output.WriteLine($"max of Employee Id: {max}");
            var response = await _controller.CreateEmployee(new Employee { FirstName = firstName });
            var content = response.Result.GetObject<Employee>();
            var count = _repo.GetByLinq(e => e.FirstName == firstName, 1, 1000).Count;
            Assert.Equal(1, count);
        }


    }
}
