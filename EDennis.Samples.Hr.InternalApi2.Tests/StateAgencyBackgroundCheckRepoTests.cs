using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Hr.InternalApi2.Models;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Hr.InternalApi2.Tests {

    public class StateBackgroundCheckRepoTests :
        ReadonlyRepoTests<StateBackgroundCheckRepo, StateBackgroundCheckView, StateBackgroundCheckContext> {
        public StateBackgroundCheckRepoTests(ITestOutputHelper output, ConfigurationClassFixture<StateBackgroundCheckRepo> fixture)
            : base(output, fixture) { }


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("StateBackgroundCheck", "EDennis.Samples.Hr.InternalApi2", "StateBackgroundCheckRepo", methodName, testScenario, testCase) {
            }
        }


        [Theory]
        [TestJson_("GetLastCheck", "CreateAndGet", "1")]
        [TestJson_("GetLastCheck", "CreateAndGet", "2")]
        public void GetLastCheck(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<StateBackgroundCheckView>("Expected");

            var actual = Repo.GetLastCheck(input);


            Assert.True(actual.IsEqualOrWrite(expected, Output));

        }
    }
}