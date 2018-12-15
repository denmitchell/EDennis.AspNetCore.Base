using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.InternalApi2.Models;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.InternalApi2.Tests {
    [Collection("Sequential")]
    public class FederalBackgroundCheckRepoUnitTests_InMemory : InMemoryUnitTest<FederalBackgroundCheckContext> {

        private FederalBackgroundCheckViewRepo _repo;
        private readonly ITestOutputHelper _output;

        public FederalBackgroundCheckRepoUnitTests_InMemory(ITestOutputHelper output) {
            _repo = new FederalBackgroundCheckViewRepo(Context);
            _output = output;
        }

        [Theory]
        [InlineData(1, "2018-01-01", "Pass")]
        [InlineData(2, "2018-02-02", "Pass")]
        [InlineData(3, "2018-03-03", "Fail")]
        [InlineData(4, "2018-04-04", "Pass")]
        public void TestCreateFederalBackgroundCheck(int employeeId, string strDateCompleted, string status) {
            var rec = _repo.GetByLinq(e => e.EmployeeId == employeeId && e.DateCompleted == DateTime.Parse(strDateCompleted) && e.Id == employeeId && e.Status == status,1,1000);
            Assert.Single(rec);
        }


    }
}
