using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
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
    public class RepoControllerDirectTests : IClassFixture<ConfigurationFactory<ColorController>>{

        private static readonly string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        private ColorController _ctlr;
        private ColorRepo _repo;
        private ITestOutputHelper _output;

        public RepoControllerDirectTests(ITestOutputHelper output, ConfigurationFactory<ColorController> fixture){

            _output = output;

            _repo = TestRepoFactory.CreateWriteableTemporalRepo<
                ColorRepo, Color, ColorDbContext, ColorHistoryDbContext, ColorController>(fixture);

            _ctlr = new ColorController(_repo, new Logger<ColorController>(new LoggerFactory()));
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
            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected")
                .OrderBy(x => x.Id);


            _ctlr.Post(input);
            var actual = _repo.Query.ToPagedList()
                .OrderBy(x => x.Id);

            Assert.True(actual.IsEqualOrWrite(expected, 2, PROPS_FILTER, _output));
        }

    }
}