using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using DbContextInterceptorApi;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;
using Lcr = DbContextInterceptorApi.Launcher;
using Lib = DbContextInterceptorApi.Lib;
using DbContextInterceptorApi.Tests;

namespace EDennis.AspNetCore.MiddlewareTests {

    /// <summary>
    /// Note: IClassFixture was not used because
    /// the individual test cases were conflicting with each
    /// (possibly, one test case was updating the configuration
    /// while another test case was calling Get).  To resolve
    /// the issue, I instantiate the LauncherFixture within each
    /// test method.  This is inefficient, but it works.
    /// </summary>
    [Collection("Sequential")]
    public class DbContextInterceptorApiTests {

        private readonly ITestOutputHelper _output;
        private readonly static BlockingCollection<bool> bc = new BlockingCollection<bool>();

        static DbContextInterceptorApiTests() {
            bc.Add(true);
        }

        public DbContextInterceptorApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        internal class TestJsonPerson : TestJsonAttribute {
            public TestJsonPerson(string methodName, string testScenario, string testCase)
                : base("DbContextInterceptorApi", "PersonController",
                      methodName, testScenario, testCase, NetCoreTestingUtilities.DatabaseProvider.Excel, "DbContextInterceptor\\TestJson.xlsx") {
            }
        }

        internal class TestJsonPosition : TestJsonAttribute {
            public TestJsonPosition(string methodName, string testScenario, string testCase)
                : base("DbContextInterceptorApi", "PositionController",
                      methodName, testScenario, testCase, NetCoreTestingUtilities.DatabaseProvider.Excel, "DbContextInterceptor\\TestJson.xlsx") {
            }
        }

        [Theory]
        [TestJsonPerson("CUD", "", "A")]
        public void PersonA(string t, JsonTestCase jsonTestCase) {

            //using var fixture = new LauncherFixture<Lib.Program, Lcr.Program>();
            //var client = fixture.HttpClient;

            var factory = new TestApis();
            var client = factory.CreateClient["DbContextInterceptorApi"]();

            client.DefaultRequestHeaders.Add(Constants.ENTRYPOINT_KEY, "TestProject");
            
            //ordinarily, include this line; however, we are testing isolation of updates by different users
            //client.DefaultRequestHeaders.Add(Constants.USER_KEY, "tester@example.org");

            _output.WriteLine($"Test case: {t}");

            Configure<Person>(client, jsonTestCase.TestCase);

            var testCases = new CrudTestCases<Person>(jsonTestCase);

            TestCreate(client, testCases, 0);
            TestCreate(client, testCases, 1);

            TestUpdate(client, testCases, 0);
            TestUpdate(client, testCases, 1);

            TestDelete(client, testCases, 0);
            TestDelete(client, testCases, 1);

            TestReset(client, testCases, 0);
            TestReset(client, testCases, 1);


        }


        [Theory]
        [TestJsonPerson("CUD", "", "B")]
        public void PersonB(string t, JsonTestCase jsonTestCase) {

            using var fixture = new LauncherFixture<Lib.Program, Lcr.Program>();
            var client = fixture.HttpClient;

            client.DefaultRequestHeaders.Add(Constants.ENTRYPOINT_KEY, "TestProject");

            //ordinarily, include this line; however, we are testing isolation of updates by different users
            //client.DefaultRequestHeaders.Add(Constants.USER_KEY, "tester@example.org");

            _output.WriteLine($"Test case: {t}");

            Configure<Person>(client, jsonTestCase.TestCase);

            var testCases = new CrudTestCases<Person>(jsonTestCase);

            TestCreate(client, testCases, 0);
            TestCreate(client, testCases, 1);

            TestUpdate(client, testCases, 0);
            TestUpdate(client, testCases, 1);

            TestDelete(client, testCases, 0);
            TestDelete(client, testCases, 1);

            TestReset(client, testCases, 0);
            TestReset(client, testCases, 1);


        }

        private void Configure<TEntity>(HttpClient client, string testCase)
        where TEntity : IHasIntegerId {

            _output.WriteLine("Executing ScopedConfiguration ...");

            //send configuration for test case
            var jcfg = File.ReadAllText($"DbContextInterceptor\\{testCase}.json");
            var status = client.Configure("", jcfg);

            _output.WriteLine("Verifing ScopedConfiguration ...");
            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

        }

        private void TestCreate<TEntity>(HttpClient client, CrudTestCases<TEntity> testCases, int userIndex)
            where TEntity : IHasIntegerId {

            _output.WriteLine($"Attempting Post/Create for User {userIndex} ...");
            var testCase = testCases[userIndex];

            foreach (var input in testCase.CreateInputs) {
                client.Post(testCase.GetPostUrl(), input);
            }

            var result = client.Get<List<TEntity>>(testCase.GetPostUrl());
            var actual = result.GetObject<List<TEntity>>();

            _output.WriteLine("Testing Post/Create ...");
            _output.WriteLine(actual.ToJsonString());

            Assert.True(actual.IsEqualOrWrite(testCase.CreateExpected, _output, true));
        }


        private void TestUpdate<TEntity>(HttpClient client, CrudTestCases<TEntity> testCases, int userIndex)
            where TEntity : IHasIntegerId {

            _output.WriteLine($"Attempting Put/Update for User {userIndex} ...");
            var testCase = testCases[userIndex];

            foreach (var input in testCase.UpdateInputs) {
                client.Put(testCase.PutDeleteUrl(input.Id), input);
            }

            var result = client.Get<List<TEntity>>(testCase.GetPostUrl());
            var actual = result.GetObject<List<TEntity>>();

            _output.WriteLine("Testing Put/Update ...");
            _output.WriteLine(actual.ToJsonString());

            Assert.True(actual.IsEqualOrWrite(testCase.UpdateExpected, _output, true));
        }


        private void TestDelete<TEntity>(HttpClient client, CrudTestCases<TEntity> testCases, int userIndex)
            where TEntity : IHasIntegerId {

            _output.WriteLine($"Attempting Delete for User {userIndex} ...");
            var testCase = testCases[userIndex];

            foreach (var id in testCase.DeleteIds) {
                client.Delete<TEntity>(testCase.PutDeleteUrl(id));
            }

            var result = client.Get<List<TEntity>>(testCase.GetPostUrl());
            var actual = result.GetObject<List<TEntity>>();

            _output.WriteLine("Testing Delete ...");
            _output.WriteLine(actual.ToJsonString());

            Assert.True(actual.IsEqualOrWrite(testCase.DeleteExpected, _output, true));
        }



        private void TestReset<TEntity>(HttpClient client, CrudTestCases<TEntity> testCases, int userIndex)
            where TEntity : IHasIntegerId {

            _output.WriteLine($"Attempting Reset for User {userIndex} ...");
            var testCase = testCases[userIndex];

            client.Get<List<TEntity>>(testCase.ResetUrl());

            var result = client.Get<List<TEntity>>(testCase.GetPostUrl());
            var actual = result.GetObject<List<TEntity>>();

            _output.WriteLine("Testing Reset ...");
            _output.WriteLine(actual.ToJsonString());

            Assert.True(actual.IsEqualOrWrite(testCase.BaseExpected, _output, true));
        }


    }
}
