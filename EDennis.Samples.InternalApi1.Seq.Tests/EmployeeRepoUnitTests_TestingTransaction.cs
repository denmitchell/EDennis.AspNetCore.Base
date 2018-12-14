using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using EDennis.Samples.InternalApi1.Models;
using EDennis.AspNetCore.Testing;
using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.InternalApi1.Tests {

    [Collection("Sequential")]
    public class EmployeeRepoUnitTests_TestingTransaction : TransactionTest<HrContext> {

        private EmployeeRepo _repo;
        private readonly ITestOutputHelper _output;

        public EmployeeRepoUnitTests_TestingTransaction(ITestOutputHelper output) {
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
            //Context.ResetValueGenerators();
        }


    }
}
