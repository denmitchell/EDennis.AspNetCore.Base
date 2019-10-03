using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.DefaultPoliciesApi;
using EDennis.Samples.DefaultPoliciesApi.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Tests {

    [Collection("DefaultPolicies Endpoint Tests")]
    public class Client3IntegrationTests : WriteableEndpointTests<Startup> {



        public Client3IntegrationTests(ITestOutputHelper output,
            ConfiguringWebApplicationFactory<Startup> factory)
        : base(output, factory, new string[] { "MockClient=EDennis.Samples.DefaultPoliciesApi.Client3" }) { 
        
        
        }

        [Fact]
        public void TestForbidden_PositionGet() {
            var statusCode = HttpClient.GetAsync<Position>("api/Position/1").Result.StatusCode;
            Assert.Equal((int)HttpStatusCode.Forbidden, statusCode);
        }

        [Fact]
        public void TestForbidden_PersonPost() {
            var statusCode = HttpClient.PostAsync<Person>("api/Person", new Person { Id = 9, Name = "George" }).Result.StatusCode;
            Assert.Equal((int)HttpStatusCode.Forbidden, statusCode);
        }


        [Fact]
        public void TestAuthorized_PersonGet() {
            var statusCode = HttpClient.GetAsync<Person>("api/Person/1").Result.StatusCode;
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void TestAuthorized_PositionPost() {
            var statusCode = HttpClient.PostAsync<Position>("api/Position", new Position { Id = 9, Title = "Director" }).Result.StatusCode;
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }


    }


    [Collection("DefaultPolicies Endpoint Tests")]
    public class Client4IntegrationTests : WriteableEndpointTests<Startup> {



        public Client4IntegrationTests(ITestOutputHelper output,
            ConfiguringWebApplicationFactory<Startup> factory)
        : base(output, factory, new string[] { "MockClient=EDennis.Samples.DefaultPoliciesApi.Client4" }) { }


        [Fact]
        public void TestBadRequest_PositionGet() {



            var statusCode = HttpClient.GetAsync<Position>("api/Position/1").Result.StatusCode;
            Assert.InRange(statusCode.Value,400,500);
        }

    }
}
