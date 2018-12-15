using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.InternalApi1.Models;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.InternalApi1.Tests {
    [Collection("Sequential")]
    public class EmployeeRepoUnitTests_InMemory : InMemoryUnitTest<HrContext> {

        private EmployeeRepo _repo;
        private readonly ITestOutputHelper _output;

        public EmployeeRepoUnitTests_InMemory(ITestOutputHelper output) {
            _repo = new EmployeeRepo(Context);
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
            _repo.Create(new Employee { FirstName = firstName });
            var count = _repo.GetByLinq(e => e.FirstName == firstName, 1, 1000).Count;
            Assert.Equal(1, count);
        }


    }
}
