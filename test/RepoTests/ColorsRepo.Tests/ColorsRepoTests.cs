﻿using Colors.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace RepoTests {
    public class ColorsRepoTests
        : TemporalRepoTests<ColorRepo, Color, ColorHistory, ColorDbContext, ColorHistoryDbContext>{
        
        public ColorsRepoTests(ITestOutputHelper output) 
            : base(output) { }

        protected string[] PropertiesToIgnore { get; }
            = new string[] { "SysStart", "SysEnd" };

        internal class TestJsonA : TestJsonAttribute {
            public TestJsonA(string methodName, string testScenario, string testCase)
                : base("ColorsRepo", "ColorRepo",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "TestJson.xlsx") {
            }
        }

        private void WriteSeed(JsonTestCase jsonTestCase) {
            var inputs = jsonTestCase.GetObject<List<Color>>($"WriteSeed");
            foreach (var input in inputs)
                Repo.Create(input);
        }

        [Fact]
        public void GetTestJsonForProject() {
            var attr = new TestJsonAttribute("ColorsRepo", "ColorRepo", "Create", "", "A", DatabaseProvider.Excel, "TestJson.xlsx");
            var cases = attr.GetData(null);
            Assert.NotNull(cases);
        }


        [Theory]
        [TestJsonA("Create", "", "A")]
        [TestJsonA("Create", "", "B")]
        public void Create(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            WriteSeed(jsonTestCase);

            var input = jsonTestCase.GetObject<Color>("Input");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");

            Repo.Create(input);

            var actual = Repo.Query.ToPagedList();

            Assert.True(actual.IsEqualAndWrite(expected,3,PropertiesToIgnore,Output,true));
        }


        [Theory]
        [TestJsonA("Update", "", "A")]
        [TestJsonA("Update", "", "B")]
        public void Update(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            WriteSeed(jsonTestCase);

            var input = jsonTestCase.GetObject<Color>("Input");
            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");
            var expectedHistory = jsonTestCase.GetObject<List<ColorHistory>>("ExpectedHistory");

            Repo.Update(input,id);

            var actual = Repo.Query.ToPagedList();
            var actualHistory = Repo.GetByIdHistory(id);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
            Assert.True(actualHistory.IsEqualAndWrite(expectedHistory, 3, PropertiesToIgnore, Output, true));
        }

        [Theory]
        [TestJsonA("Delete", "", "A")]
        [TestJsonA("Delete", "", "B")]
        public void Delete(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine($"Test case: {t}");

            WriteSeed(jsonTestCase);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Color>>("Expected");
            var expectedHistory = jsonTestCase.GetObject<List<ColorHistory>>("ExpectedHistory");

            Repo.Delete(id);

            var actual = Repo.Query.ToPagedList();
            var actualHistory = Repo.GetByIdHistory(id);

            Assert.True(actual.IsEqualAndWrite(expected, 3, PropertiesToIgnore, Output, true));
            Assert.True(actualHistory.IsEqualAndWrite(expectedHistory, 3, PropertiesToIgnore, Output, true));
        }


    }
}
