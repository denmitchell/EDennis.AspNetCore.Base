using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.Hr.InternalApi2.Models;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.InternalApi2.Tests {
    public class AgencyOnlineCheckRepoUnitTests_InMemory :
        WriteableRepoTests<AgencyOnlineCheckRepo, AgencyOnlineCheck, AgencyOnlineCheckContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public AgencyOnlineCheckRepoUnitTests_InMemory(ITestOutputHelper output,
            WriteableClassFixture fixture) : base(output, fixture) { }


        [Theory]
        [InlineData(1, "2018-12-01", "Pass")]
        [InlineData(2, "2018-12-02", "Fail")]
        [InlineData(3, "2018-12-03", "Pass")]
        [InlineData(4, "2018-12-04", "Fail")]
        public void TestCreateAgencyOnlineCheck(int employeeId, string strDateCompleted, string status) {

            var preCount = Repo.GetScalarFromDapper<int>("select count(*) recs from AgencyOnlineCheck");

            Repo.Create(new AgencyOnlineCheck {
                EmployeeId = employeeId,
                DateCompleted = DateTime.Parse(strDateCompleted),
                Status = status
            });

            var postCount = Repo.GetScalarFromDapper<int>("select count(*) recs from AgencyOnlineCheck");

            Assert.Equal(preCount + 1, postCount);

            var targetRec = Repo.Query
                .OrderBy(e => e.Id)
                .LastOrDefault();

            Assert.Equal(employeeId, targetRec.EmployeeId);
            Assert.Equal(DateTime.Parse(strDateCompleted), targetRec.DateCompleted);
            Assert.Equal(status, targetRec.Status);


        }


    }
}
