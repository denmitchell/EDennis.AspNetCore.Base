using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.Hr.InternalApi2.Models;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.InternalApi2.Tests {
    public class FederalBackgroundCheckRepoUnitTests_InMemory :
        InMemoryRepoTests<FederalBackgroundCheckRepo, FederalBackgroundCheckView, FederalBackgroundCheckContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public FederalBackgroundCheckRepoUnitTests_InMemory(ITestOutputHelper output,
            InMemoryClassFixture fixture) : base(output, fixture) { }


        [Theory]
        [InlineData(1, "2018-01-01", "Pass")]
        [InlineData(2, "2018-02-02", "Pass")]
        [InlineData(3, "2018-03-03", "Fail")]
        [InlineData(4, "2018-04-04", "Pass")]
        public void GetFederalBackgroundCheck(int employeeId, string strDateCompleted, string status) {
            var preCount = _repo.GetScalarFromDapper<int>("select count(*) recs from AgencyOnlineCheck");

            var check = _repo.GetLastCheck(employeeId);

            Assert.Equal(DateTime.Parse(strDateCompleted), check.DateCompleted);
            Assert.Equal(status, check.Status);

        }


    }
}
