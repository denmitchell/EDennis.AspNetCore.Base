using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.Hr.InternalApi2.Controllers;
using EDennis.Samples.Hr.InternalApi2.Models;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.InternalApi2.Tests {

    public class AgencyOnlineCheckControllerUnitTests_Clone :
        CloneRepoTests<AgencyOnlineCheckRepo, AgencyOnlineCheck, AgencyOnlineCheckContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        private AgencyOnlineCheckController _controller;

        public AgencyOnlineCheckControllerUnitTests_Clone(
                ITestOutputHelper output, CloneClassFixture fixture) 
            : base (output,fixture){
            _controller = new AgencyOnlineCheckController(_repo);
        }

        [Theory]
        [InlineData(1, "2018-12-01", "Pass")]
        [InlineData(2, "2018-12-02", "Fail")]
        [InlineData(3, "2018-12-03", "Pass")]
        [InlineData(4, "2018-12-04", "Fail")]
        public void TestCreateAgencyOnlineCheck(int employeeId, string strDateCompleted, string status) {

            var preCount = _repo.GetScalarFromDapper<int>("select count(*) recs from AgencyOnlineCheck");

            _controller.Post(new AgencyOnlineCheck {
                EmployeeId = employeeId,
                DateCompleted = DateTime.Parse(strDateCompleted),
                Status = status
            });

            var postCount = _repo.GetScalarFromDapper<int>("select count(*) recs from AgencyOnlineCheck");

            Assert.Equal(preCount + 1, postCount);

            var targetRec = _repo.Query
                .OrderBy(e => e.Id)
                .LastOrDefault();

            Assert.Equal(employeeId, targetRec.EmployeeId);
            Assert.Equal(DateTime.Parse(strDateCompleted), targetRec.DateCompleted);
            Assert.Equal(status, targetRec.Status);


        }


    }
}
