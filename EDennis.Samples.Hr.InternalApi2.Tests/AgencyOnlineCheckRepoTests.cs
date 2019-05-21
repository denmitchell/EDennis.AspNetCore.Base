using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Hr.InternalApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.InternalApi2.Tests {
    public class AgencyOnlineCheckRepoTests :
        WriteableRepoTests<AgencyOnlineCheckRepo, AgencyOnlineCheck, AgencyOnlineCheckContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public AgencyOnlineCheckRepoTests(ITestOutputHelper output,
            ConfigurationFactory<AgencyOnlineCheckRepo> fixture) : base(output, fixture) { }


        [Theory]
        [InlineData(1, "2018-12-01", "Pass")]
        [InlineData(2, "2018-12-02", "Fail")]
        [InlineData(3, "2018-12-03", "Pass")]
        [InlineData(4, "2018-12-04", "Fail")]
        public void TestCreateAgencyOnlineCheck(int employeeId, string strDateCompleted, string status) {

            Repo.Create(new AgencyOnlineCheck {
                EmployeeId = employeeId,
                DateCompleted = DateTime.Parse(strDateCompleted),
                Status = status
            });

            var targetRec = Repo.Query
                .OrderBy(e => e.Id)
                .LastOrDefault();

            Assert.Equal(employeeId, targetRec.EmployeeId);
            Assert.Equal(DateTime.Parse(strDateCompleted), targetRec.DateCompleted);
            Assert.Equal(status, targetRec.Status);

        }

        //'EDennis.Samples.Hr.InternalApi2', 'AgencyOnlineRepo', 'Update', 'UpdateAndGetMultiple'

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase) 
                : base("AgencyOnlineCheck", "EDennis.Samples.Hr.InternalApi2", 
                      "AgencyOnlineRepo", 
                      methodName, testScenario, testCase,
                      "(LocalDb)\\MSSQLLocalDb", "_","TestJson") {
            }
        }


        [Theory]
        [TestJson_("Update", "UpdateAndGetMultiple", "1")]
        [TestJson_("Update", "UpdateAndGetMultiple", "2")]
        public void TestUpdateAgencyOnlineCheck(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine($"Instance Name:{InstanceName}");
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<AgencyOnlineCheck>("Input");
            var expected = 
                jsonTestCase.GetObject<List<AgencyOnlineCheck>>("Expected")
                .OrderBy(x=>x.Id);

            Repo.ScopeProperties.User = input.SysUser;

            Repo.Update(input, id);
            var actual = Repo.Context.AgencyOnlineChecks.ToList().OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


        [Theory]
        [TestJson_("Update", "UpdateAndGetMultiplePatch", "1")]
        [TestJson_("Update", "UpdateAndGetMultiplePatch", "2")]
        public void TestUpdatePatchAgencyOnlineCheck(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine($"Instance Name:{InstanceName}");
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<dynamic>("Input");
            var expected =
                jsonTestCase.GetObject<List<AgencyOnlineCheck>>("Expected")
                .OrderBy(x => x.Id);

            Repo.ScopeProperties.User = input.SysUser;

            Repo.Update(input, id);
            var actual = Repo.Context.AgencyOnlineChecks.ToList().OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, PROPS_FILTER, Output));
        }


    }
}
