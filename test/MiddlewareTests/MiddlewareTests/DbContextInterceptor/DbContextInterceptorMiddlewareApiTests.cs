using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.DbContextInterceptorMiddlewareApi;
using EDennis.Samples.DbContextInterceptorMiddlewareApi.Lib;
using EDennis.Samples.DbContextInterceptorMiddlewareApi.Tests;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.MiddlewareTests {
    /// <summary>
    /// Note: IClassFixture was not used because
    /// the individual test cases were conflicting with each
    /// (possibly, one test case was updating the configuration
    /// while another test case was calling Get).  To resolve
    /// the issue, I instantiate the TestApis within the
    /// test method.  This is inefficient, but it works.
    /// </summary>
    [Collection("Sequential")]
    public class DbContextInterceptorMiddlewareApiTests {

        private readonly ITestOutputHelper _output;

        public DbContextInterceptorMiddlewareApiTests(
            ITestOutputHelper output) {
            _output = output;
        }


        internal class TestJsonPerson : TestJsonAttribute {
            public TestJsonPerson(string methodName, string testScenario, string testCase)
                : base("DbContextInterceptorApi", "PersonController",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "DbContextInterceptor\\TestJson.xlsx") {
            }
        }

        internal class TestJsonPosition : TestJsonAttribute {
            public TestJsonPosition(string methodName, string testScenario, string testCase)
                : base("DbContextInterceptorApi", "PositionController",
                      methodName, testScenario, testCase, DatabaseProvider.Excel, "DbContextInterceptor\\TestJson.xlsx") {
            }
        }

        [Theory]
        [TestJsonPerson("CUD", "", "A")]
        [TestJsonPerson("CUD", "", "B")]
        public void Person(string t, JsonTestCase jsonTestCase) {

            using var factory = new TestApis();
            var client = factory.CreateClient["DbContextInterceptorApi"]();

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

            Assert.True(actual.IsEqualOrWrite(testCase.DeleteExpected, _output, true));
        }



        private void TestReset<TEntity>(HttpClient client, CrudTestCases<TEntity> testCases, int userIndex)
            where TEntity : IHasIntegerId {

            _output.WriteLine($"Attempting Reset for User {userIndex} ...");
            var testCase = testCases[userIndex];

            client.Post<TEntity>(testCase.ResetUrl(),default);

            var result = client.Get<List<TEntity>>(testCase.GetPostUrl());
            var actual = result.GetObject<List<TEntity>>();

            _output.WriteLine("Testing Reset ...");

            Assert.True(actual.IsEqualOrWrite(testCase.DeleteExpected, _output, true));
        }


    }
}
