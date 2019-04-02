using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Tests;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Colors.InternalApi.Controllers;
using EDennis.Samples.Colors.InternalApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Tests {
    public class RepoControllerDirectTests : WriteableTemporalRepoTests<ColorRepo, Color, ColorDbContext, ColorHistoryDbContext> {

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        private ColorController _ctlr;

        public RepoControllerDirectTests(ITestOutputHelper output, ConfigurationClassFixture<ColorRepo> fixture)
            : base(output, fixture) {

            ScopeProperties.User = "moe@stooges.org";

            _ctlr = new ColorController(Repo, new Logger<ColorController>(new LoggerFactory()));
        }


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("ColorDb", "EDennis.Samples.Colors.InternalApi", "ColorRepo", methodName, testScenario, testCase) {
            }
        }


        [Theory]
        [TestJson_("Create", "SqlRepo", "brown")]
        [TestJson_("Create", "SqlRepo", "orange")]
        public void Create(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);


            _ctlr.Post(input);
            var actual = Repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, Output));
        }

    }
}