using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.DefaultPoliciesConfigsApi.Tests;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.ConfigTests {
    [Collection("Sequential")]

    public class DefaultPoliciesConfigsApiTests :
        IClassFixture<TestApis> {


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public DefaultPoliciesConfigsApiTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        private static readonly string[] defaultPoliciesUrl = new string[] {
            "Person/User","Person/Admin","Position/User","Position/Admin"
        };

        private static readonly string[] defaultPoliciesExpected = new string[] {
            "User","Admin","User","Admin"
        };

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void TestUrls(int testCase) {

            var client = _factory.CreateClient["DefaultPoliciesApi"]();
            var result = client.Get<string>(defaultPoliciesUrl[testCase]);

            if (testCase < 3) {
                string actual = (string)result.Value;
                _output.WriteLine(actual);
                Assert.Equal($"Hello, {defaultPoliciesExpected[testCase]}!", actual);
            } else {
                int actual = result.StatusCode.Value;
                _output.WriteLine(actual.ToString());
                Assert.Equal(401, actual);
            }

        }
          



    }
}