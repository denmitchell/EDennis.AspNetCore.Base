using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Hr.InternalApi2.Models;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.InternalApi2.Tests {
    public class AgencyInvestigatorCheckRepoTests :
        WriteableTemporalRepoTests<AgencyInvestigatorCheckRepo,
            AgencyInvestigatorCheck,
            AgencyInvestigatorCheckContext,
            AgencyInvestigatorCheckHistoryContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public AgencyInvestigatorCheckRepoTests(ITestOutputHelper output,
            ConfigurationFactory<AgencyInvestigatorCheckRepo> fixture) : base(output, fixture) { }

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("AgencyInvestigatorCheck","EDennis.Samples.Hr.InternalApi2","AgencyInvestigatorCheckRepo", methodName, testScenario, testCase) {
            }
        }

        [Theory]
        [TestJson_("GetLastCheck", "CreateAndGet", "1")]
        [TestJson_("GetLastCheck", "CreateAndGet", "2")]
        public void GetLastCheck(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<AgencyInvestigatorCheck>("Expected");

            var actual = Repo.GetLastCheck(input);


            Assert.True(actual.IsEqualOrWrite(expected, Output));

        }



    }
}
