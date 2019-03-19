using System;
using System.Collections.Generic;
using System.Text;
using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.Hr.InternalApi2.Models;
using Xunit.Abstractions;
using Xunit;
using EDennis.NetCoreTestingUtilities;
using System.Linq;
using EDennis.NetCoreTestingUtilities.Extensions;

namespace EDennis.Samples.Hr.InternalApi2.Tests {
    public class StateBackgroundCheckRepoTests :
        ReadonlyRepoTests<StateBackgroundCheckRepo, StateBackgroundCheckView, StateBackgroundCheckContext> {
        public StateBackgroundCheckRepoTests(ITestOutputHelper output, ConfigurationClassFixture<StateBackgroundCheckRepo> fixture)
            : base(output, fixture) { }


        /// <summary>
        /// Optional internal class ... reduced the number of parameters in TestJson attribute
        /// by specifying constant parameter values for className and testJsonConfigPath here
        /// </summary>
        internal class TestJsonSpecific : TestJsonAttribute {
            public TestJsonSpecific(string methodName, string testScenario, string testCase)
                : base("StateBackgroundCheckRepo", methodName, testScenario, testCase, "TestJsonConfigs\\StateBackgroundCheck.json") {
            }
        }


        [Theory]
        [TestJsonSpecific("GetLastCheck", "CreateAndGet", "1")]
        [TestJsonSpecific("GetLastCheck", "CreateAndGet", "2")]
        public void GetLastCheck(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<int>("Input");
            var expected = jsonTestCase.GetObject<StateBackgroundCheckView>("Expected");

            var actual = Repo.GetLastCheck(input);


            Assert.True(actual.IsEqualOrWrite(expected, Output));

        }
    }
}