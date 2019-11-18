using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.Hr.InternalApi2.Models;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.InternalApi2.Tests {
    public class FederalBackgroundCheckRepoTests :
        ReadonlyTemporalRepoTests<FederalBackgroundCheckRepo, 
            FederalBackgroundCheckView, 
            FederalBackgroundCheckContext, 
            FederalBackgroundCheckHistoryContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public FederalBackgroundCheckRepoTests(ITestOutputHelper output,
            ConfigurationFactory<FederalBackgroundCheckRepo> fixture) : base(output, fixture) { }


        [Theory]
        [InlineData(1, "2018-01-01", "Pass")]
        [InlineData(2, "2018-02-02", "Pass")]
        [InlineData(3, "2018-03-03", "Fail")]
        [InlineData(4, "2018-04-04", "Pass")]
        public void GetFederalBackgroundCheck(int employeeId, string strDateCompleted, string status) {

            var check = Repo.GetLastCheck(employeeId);

            Assert.Equal(DateTime.Parse(strDateCompleted), check.DateCompleted);
            Assert.Equal(status, check.Status);

        }

        [Theory]
        [InlineData(1,2016,6,1, "Pass")]
        public void GetFederalBackgroundCheckAsOf(int id, int year, int month, int day, string status) {

            var history = Repo.QueryAsOf(
                new DateTime(year,month,day),
                e=>e.Id == id
                ).FirstOrDefault();

            Assert.Equal(status,history.Status);
        }



        [Theory]
        [InlineData(1,4)]
        public void GetFederalBackgroundHistory(int id, int count) {

            var history = Repo.QueryAsOf(
                DateTime.MinValue,
                DateTime.MaxValue,
                e => e.Id == id,
                1,100,
                desc=>desc.SysStart
                );

            Assert.Equal(count,history.Count);
        }

    }
}
