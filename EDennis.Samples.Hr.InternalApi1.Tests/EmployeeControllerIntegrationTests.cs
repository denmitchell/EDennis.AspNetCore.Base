using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.InternalApi1.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;


namespace EDennis.Samples.Hr.InternalApi1.Tests {

    public class EmployeeControllerIntegrationTests
        : WriteableTemporalEndpointTests<Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };

        public EmployeeControllerIntegrationTests(
            ITestOutputHelper output, ConfiguringWebApplicationFactory<Startup> factory)
            : base(output, factory) { }



        [Theory]
        [InlineData("Regis")]
        [InlineData("Wink")]
        [InlineData("Moe")]
        [InlineData("Larry")]
        [InlineData("Curly")]
        public void TestCreateAndGetEmployee(string firstName) {

            Output.WriteLine($"Instance Name:{InstanceName}");

            HttpClient.Post("iapi/employee", new Employee { FirstName = firstName });
            var employee = HttpClient.Get<Employee>("iapi/employee/5").Value;

            Assert.Equal(firstName, employee.FirstName);

        }


    }
}
