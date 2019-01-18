﻿using System.Linq;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Web.Launcher.Tests {

    public class ApiLauncherTests : IClassFixture<ApiFixture> {

        private readonly ITestOutputHelper _output;

        private readonly ApiFixture _fixture;

        public ApiLauncherTests(ITestOutputHelper output, ApiFixture fixture) {
            _output = output;
            _fixture = fixture;
        }


        [Fact]
        public async void StartCheckStopMultipleApis() {

            var client = new HttpClient();
            foreach(var er in _fixture.ExpectedResponses) {
                var url = _fixture.Urls.Where(u=>u.Port==er.Port).FirstOrDefault().Url;

                var response = await client.GetAsync(url);
                var hello = await response.Content.ReadAsStringAsync();

                Assert.Equal(er.ExpectedResponse, hello);
                _output.WriteLine(hello);
            }

        }

    }
}
