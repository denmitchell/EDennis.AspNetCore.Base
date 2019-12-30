using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.DbContextInterceptorMiddlewareApi;
using EDennis.Samples.DbContextInterceptorMiddlewareApi.Lib;
using EDennis.Samples.DbContextInterceptorMiddlewareApi.Tests;
using System.Collections.Generic;
using System.IO;
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
                : base("DbContextInterceptorApi", "PersonController",
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

            _output.WriteLine("Executing ScopedConfiguration ...");

            //send configuration for test case
            var jcfg = File.ReadAllText($"Person\\{jsonTestCase.TestCase}.json");
            var status = client.Configure("", jcfg);

            _output.WriteLine("Verifing ScopedConfiguration ...");

            //make sure that configuration was successful
            Assert.Equal((int)System.Net.HttpStatusCode.OK, status.GetStatusCode());

            var user = jsonTestCase.GetObject<string>("User");
            var createInput = jsonTestCase.GetObject<Person>("CreateInput");
            var updateInput = jsonTestCase.GetObject<Person>("UpdateInput");
            var id = createInput.Id;
            
            var createExpected = jsonTestCase.GetObject<List<Person>>("CreateExpected");
            var updateExpected = jsonTestCase.GetObject<List<Person>>("UpdateExpected");
            var deleteExpected = jsonTestCase.GetObject<List<Person>>("DeleteExpected");

            var getUrl = $"Person?X-User={user}X-Developer={user}";
            var getUrlOtherUser = $"Person?X-User=someOtherUser";

            _output.WriteLine("Attempting POST ...");

            client.Post(getUrl, createInput);
            
            var createResult = client.Get<List<Person>>(getUrl);
            var createActual = createResult.GetObject<List<Person>>();

            _output.WriteLine("Verifying ScopedConfiguration ...");

            Assert.True(createActual.IsEqualOrWrite(createExpected, _output, true));


            var otherUserResult = client.Get<List<Person>>(getUrlOtherUser);
            var otherUserActual = otherUserResult.GetObject<List<Person>>();

            Assert.True(otherUserActual.IsEqualOrWrite(deleteExpected, _output, true));

            var url = $"Person/{updateInput.Id}?X-User={user}";
            client.Put(url, updateInput);

            var updateResult = client.Get<List<Person>>(url);
            var updateActual = updateResult.GetObject<List<Person>>();

            Assert.True(updateActual.IsEqualOrWrite(updateExpected, _output, true));

            otherUserResult = client.Get<List<Person>>(getUrlOtherUser);
            otherUserActual = otherUserResult.GetObject<List<Person>>();

            Assert.True(otherUserActual.IsEqualOrWrite(deleteExpected, _output, true));

            url = $"Person/{updateInput.Id}?X-User={user}";
            client.Delete<Person>(url);

            var deleteResult = client.Get<List<Person>>(url);
            var deleteActual = updateResult.GetObject<List<Person>>();

            Assert.True(deleteActual.IsEqualOrWrite(deleteExpected, _output, true));

            url = $"Person";
            client.Post(url, createInput);

            createResult = client.Get<List<Person>>(url);
            createActual = createResult.GetObject<List<Person>>();

            Assert.True(createActual.IsEqualOrWrite(createExpected, _output, true));

            url = $"Person?X-Testing-Reset=jack@hill.org";
        
            var resetResult = client.Get<List<Person>>(url);
            var resetActual = resetResult.GetObject<List<Person>>();

            Assert.True(resetActual.IsEqualOrWrite(deleteExpected, _output, true));

        }



    }
}
