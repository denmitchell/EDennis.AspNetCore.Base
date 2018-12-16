using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.InternalApi2.Controllers;
using EDennis.Samples.InternalApi2.Models;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.InternalApi2.Tests {

    public class AgencyOnlineCheckControllerUnitTests_InMemory : InMemoryUnitTest<AgencyOnlineCheckContext> {

        private AgencyOnlineCheckController _controller;
        private AgencyOnlineCheckRepo _repo;
        private readonly ITestOutputHelper _output;

        public AgencyOnlineCheckControllerUnitTests_InMemory(ITestOutputHelper output) {
            _repo = new AgencyOnlineCheckRepo(Context);
            _controller = new AgencyOnlineCheckController(_repo);
            _output = output;
        }

        [Theory]
        [InlineData(1, "2018-12-01", "Pass")]
        [InlineData(2, "2018-12-02", "Fail")]
        [InlineData(3, "2018-12-03", "Pass")]
        [InlineData(4, "2018-12-04", "Fail")]
        public void TestCreateAgencyOnlineCheck(int employeeId, string strDateCompleted, string status) {
            var max = Context.GetMaxKeyValue<AgencyOnlineCheck>();
            _output.WriteLine($"max of AgencyOnlineCheck Id: {max}");
            _controller.Post(new AgencyOnlineCheck { EmployeeId = employeeId, DateCompleted = DateTime.Parse(strDateCompleted), Status = status });
            var check = _repo.GetByLinq(e => e.EmployeeId == employeeId && e.DateCompleted == DateTime.Parse(strDateCompleted) && e.Id == (max + 1), 1, 1000)[0];
            _output.WriteLine($"Id: {check.Id}, NamedInstance: {NamedInstance}");
            var count = _repo.GetByLinq(e => e.EmployeeId == employeeId && e.DateCompleted == DateTime.Parse(strDateCompleted) && e.Id == (max + 1), 1, 1000).Count;
            Assert.Equal(1, count);
        }


    }
}
