using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.TodoApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.TodoApi.Tests {
    public class HttpClientExtensionTests {

        private static WebApplicationFactory<Startup> _factory;
        private static HttpClient _client;
        private string _dbName;

        private string[] _propsToIgnore = new string[] { "SysStart", "SysEnd" };

        static HttpClientExtensionTests() {
            _factory = new WebApplicationFactory<Startup>();
        }

        private readonly ITestOutputHelper _output;

        public HttpClientExtensionTests(ITestOutputHelper output) {
            _output = output;
            _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5000/iapi/task");

            _dbName = Guid.NewGuid().ToString();
        }


        [Theory]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGet", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGet", testCase: "B")]
        public void PostAndGet(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Task>("Input");
            var expected = jsonTestCase.GetObject<Task>("Expected");

            var actual = _client.PostAndGetForTest(
                input, DbType.InMemory, _dbName, id);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }

        [Theory]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGetMultiple", testCase: "B")]
        public void PostAndGetMultiple(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Task>("Input");
            var expected = jsonTestCase.GetObject<List<Task>>("Expected");

            var actual = _client.PostAndGetMultipleForTest(
                input, DbType.InMemory, _dbName);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }

        [Theory]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "B")]
        public void PutAndGet(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Task>("Input");
            var expected = jsonTestCase.GetObject<Task>("Expected");

            var actual = _client.PutAndGetForTest(
                input, DbType.InMemory, _dbName, id);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }


        [Theory]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGetMultiple", testCase: "B")]
        public void PutAndGetMultiple(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Task>("Input");
            var expected = jsonTestCase.GetObject<List<Task>>("Expected");

            var actual = _client.PutAndGetMultipleForTest(
                input, DbType.InMemory, _dbName, id);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }

        [Theory]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public void DeleteAndGetMultiple(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Task>>("Expected");

            var actual = _client.DeleteAndGetMultipleForTest<Task>(
                DbType.InMemory, _dbName, id);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }

    }
}
