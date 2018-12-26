using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.TodoApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
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
        public void Post(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<Models.Task>("Expected");

            var guid = Guid.NewGuid().ToString();

            _client.DefaultRequestHeaders.Add("X-Testing-UseInMemory", guid);
            _client.Post(input);

            var actual = _client.Get<Models.Task>(id);
            _client.DefaultRequestHeaders.Add("X-Testing-DropInMemory", guid);
            _client.Head();

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }



        [Theory]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGet", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGet", testCase: "B")]
        public async System.Threading.Tasks.Task PostAsync(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<Models.Task>("Expected");

            var guid = Guid.NewGuid().ToString();

            _client.DefaultRequestHeaders.Add("X-Testing-UseInMemory", guid);
            await _client.PostAsync(input);

            var actual = await _client.GetAsync<Models.Task>(id);
            _client.DefaultRequestHeaders.Add("X-Testing-DropInMemory", guid);
            await _client.HeadAsync();

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

            return;
        }



        [Theory]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGet", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGet", testCase: "B")]
        public void PostAndGet(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<Models.Task>("Expected");

            var actual = _client.PostAndGetForTest(
                input, DbType.InMemory, _dbName, id);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }


        [Theory]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGet", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGet", testCase: "B")]
        public void TryPostAndGet(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<Models.Task>("Expected");

            var response = _client.TryPostAndGetForTest(
                input, DbType.InMemory, _dbName, id);

            Assert.True(response[0].StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response[1].StatusCode == System.Net.HttpStatusCode.OK);

            var actual = response[1].Content.ReadAsAsync<Models.Task>().Result;
            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }



        [Theory]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGetMultiple", testCase: "B")]
        public void PostAndGetMultiple(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<List<Models.Task>>("Expected");

            var actual = _client.PostAndGetMultipleForTest(
                input, DbType.InMemory, _dbName);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }


        [Theory]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Post", testScenario: "PostAndGetMultiple", testCase: "B")]
        public void TryPostAndGetMultiple(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<List<Models.Task>>("Expected");

            var response = _client.TryPostAndGetMultipleForTest(
                input, DbType.InMemory, _dbName);

            Assert.True(response[0].StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response[1].StatusCode == System.Net.HttpStatusCode.OK);

            var actual = response[1].Content.ReadAsAsync<List<Models.Task>>().Result;

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }



        [Theory]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "B")]
        public void Put(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<Models.Task>("Expected");

            var guid = Guid.NewGuid().ToString();

            _client.DefaultRequestHeaders.Add("X-Testing-UseInMemory", guid);
            _client.Put(input,id);

            var actual = _client.Get<Models.Task>(id);
            _client.DefaultRequestHeaders.Add("X-Testing-DropInMemory", guid);
            _client.Head();

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }



        [Theory]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "B")]
        public async System.Threading.Tasks.Task PutAsync(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<Models.Task>("Expected");

            var guid = Guid.NewGuid().ToString();

            _client.DefaultRequestHeaders.Add("X-Testing-UseInMemory", guid);
            await _client.PutAsync(input,id);

            var actual = await _client.GetAsync<Models.Task>(id);
            _client.DefaultRequestHeaders.Add("X-Testing-DropInMemory", guid);
            await _client.HeadAsync();

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

            return;
        }




        [Theory]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "B")]
        public void PutAndGet(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<Models.Task>("Expected");

            var actual = _client.PutAndGetForTest(
                input, DbType.InMemory, _dbName, id);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }


        [Theory]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGet", testCase: "B")]
        public void TryPutAndGet(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<Models.Task>("Expected");

            var response = _client.TryPutAndGetForTest(
                input, DbType.InMemory, _dbName, id);

            Assert.True(response[0].StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response[1].StatusCode == System.Net.HttpStatusCode.OK);

            var actual = response[1].Content.ReadAsAsync<Models.Task>().Result;
            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }


        [Theory]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGetMultiple", testCase: "B")]
        public void PutAndGetMultiple(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<List<Models.Task>>("Expected");

            var actual = _client.PutAndGetMultipleForTest(
                input, DbType.InMemory, _dbName, id);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }


        [Theory]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Put", testScenario: "PutAndGetMultiple", testCase: "B")]
        public void TryPutAndGetMultiple(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<Models.Task>("Input");
            var expected = jsonTestCase.GetObject<List<Models.Task>>("Expected");

            var response = _client.TryPutAndGetMultipleForTest(
                input, DbType.InMemory, _dbName, id);

            Assert.True(response[0].StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response[1].StatusCode == System.Net.HttpStatusCode.OK);

            var actual = response[1].Content.ReadAsAsync<List<Models.Task>>().Result;

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }




        [Theory]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public void Delete(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Models.Task>>("Expected");

            var guid = Guid.NewGuid().ToString();

            _client.DefaultRequestHeaders.Add("X-Testing-UseInMemory", guid);
            _client.Delete(id);

            var actual = _client.Get<List<Models.Task>>();
            _client.DefaultRequestHeaders.Add("X-Testing-DropInMemory", guid);
            _client.Head();

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));
        }



        [Theory]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public async System.Threading.Tasks.Task DeleteAsync(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Models.Task>>("Expected");

            var guid = Guid.NewGuid().ToString();

            _client.DefaultRequestHeaders.Add("X-Testing-UseInMemory", guid);
            await _client.DeleteAsync(id);

            var actual = await _client.GetAsync<List<Models.Task>>();
            _client.DefaultRequestHeaders.Add("X-Testing-DropInMemory", guid);
            await _client.HeadAsync();

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

            return;
        }


        [Theory]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public void DeleteAndGetMultiple(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Models.Task>>("Expected");

            var actual = _client.DeleteAndGetMultipleForTest<Models.Task>(
                DbType.InMemory, _dbName, id);

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }

        [Theory]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "A")]
        [TestJson(className: "TaskController", methodName: "Delete", testScenario: "DeleteAndGetMultiple", testCase: "B")]
        public void TryDeleteAndGetMultiple(string t, JsonTestCase jsonTestCase) {

            _output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<Models.Task>>("Expected");

            var response = _client.TryDeleteAndGetMultipleForTest<Models.Task>(
                DbType.InMemory, _dbName, id);

            Assert.True(response[0].StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response[1].StatusCode == System.Net.HttpStatusCode.OK);

            var actual = response[1].Content.ReadAsAsync<List<Models.Task>>().Result;

            Assert.True(actual.IsEqualOrWrite(expected, _propsToIgnore, _output));

        }


    }
}
