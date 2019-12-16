using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.DbContextConfigsApi.Tests;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    public class DbContextConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public DbContextConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }

        private static readonly string[] _databaseProviderName = new string[] {
            "Microsoft.EntityFrameworkCore.SqlServer", "Microsoft.EntityFrameworkCore.Sqlite", "Microsoft.EntityFrameworkCore.InMemory"
        };

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void TestDbContexts(int dbContextSuffix) {

            var client = _factory.CreateClient["DbContextConfigsApi"]();
            var result = client.Get<Dictionary<string,string>>($"DbContext{dbContextSuffix}");
            Dictionary<string,string> obj = (Dictionary<string, string>)result.Value;

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);

            Assert.Equal(_databaseProviderName[dbContextSuffix-1], obj["DatabaseProviderName"] );
            Assert.Equal("EDennis.Samples.DbContextConfigsApi.Person,EDennis.Samples.DbContextConfigsApi.Position", obj["EntityTypes"]);
            Assert.Equal("3", obj["PersonCount"]);
            Assert.Equal("2", obj["PositionCount"]);

        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void TestRepos(int repoSuffix) {

            var client = _factory.CreateClient["DbContextConfigsApi"]();
            var result = client.Get<Dictionary<string, string>>($"Repo{repoSuffix}");
            Dictionary<string, string> obj = (Dictionary<string, string>)result.Value;

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            _output.WriteLine(json);

            Assert.Equal("Information", obj["PersonRepoScopedLoggerLevel"]);
            Assert.Null(obj["PersonRepoScopePropertiesUser"]);
            Assert.Equal("3", obj["PersonCount"]);
            Assert.Equal("Information", obj["PositionRepoScopedLoggerLevel"]);
            Assert.Null(obj["PositionRepoScopePropertiesUser"]);
            Assert.Equal("2", obj["PositionCount"]);

        }



    }
}