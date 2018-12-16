using EDennis.Samples.InternalApi1;
using EDennis.Samples.InternalApi1.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Reflection.Tests {
    public class AddDbContextPoolTests {

        private static WebApplicationFactory<Startup> _factory;
        private static HttpClient _client;

        static AddDbContextPoolTests() {
            _factory = new WebApplicationFactory<Startup>();
        }

        private readonly ITestOutputHelper _output;

        public AddDbContextPoolTests(ITestOutputHelper output) {
            _output = output;
            _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5000/iapi/employee");
        }


        [Theory]
        [InlineData(1, "Bob")]
        [InlineData(2, "Monty")]
        [InlineData(3, "Drew")]
        [InlineData(4, "Wayne")]
        public async Task AddDbContextPool(int id, string firstName) {
            var response = await _client.GetAsync($"{_client.BaseAddress}/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var employee = JToken.Parse(json).ToObject<Employee>();
            Assert.Equal(firstName, employee.FirstName);
        }



    }
}
