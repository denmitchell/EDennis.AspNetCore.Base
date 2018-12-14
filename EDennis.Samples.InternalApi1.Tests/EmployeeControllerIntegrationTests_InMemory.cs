using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Testing;
using EDennis.Samples.InternalApi1.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;


namespace EDennis.Samples.InternalApi1.Tests {

    [Collection("Sequential")]
    public class EmployeeControllerIntegrationTests_InMemory {

        private static WebApplicationFactory<Startup> _factory;        
        private static HttpClient _client;

        static EmployeeControllerIntegrationTests_InMemory() {
            _factory = new WebApplicationFactory<Startup>();
        }

        private readonly ITestOutputHelper _output;

        public EmployeeControllerIntegrationTests_InMemory(ITestOutputHelper output) {
            _output = output;
            _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5000/iapi/employee");
        }


        [Theory]
        [InlineData("Regis")]
        [InlineData("Wink")]
        [InlineData("Moe")]
        [InlineData("Larry")]
        [InlineData("Curly")]
        public void TestCreateEmployee(string firstName) {

            var dbName = Guid.NewGuid().ToString();

            var actual = _client.PostAndGet(
                new Employee { FirstName = firstName },
                DbType.InMemory, dbName, new object[] { 5 });

            Assert.Equal(firstName, actual.FirstName);

        }


    }
}
