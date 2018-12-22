using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Web.Launcher.Tests {

    public class ExpectedResponses {
        public string ApiName { get; set; }
        public string ExpectedResponse { get; set; }
    }

    public class ApiFixture : IDisposable {

        public List<ExpectedResponses> ExpectedResponses
            = new List<ExpectedResponses>();
        public ApiLauncher Launcher { get; set; }

        public ApiFixture() {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            Launcher = new ApiLauncher(config);

            var apiSections = config.GetSection("ExpectedResponses").GetChildren();

            foreach (var apiSection in apiSections) {
                ExpectedResponses.Add(
                    new ExpectedResponses {
                        ApiName = apiSection.Key,
                        ExpectedResponse = apiSection.Value
                    });
            }

        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    Launcher.Dispose();
                }
                disposedValue = true;
            }
        }
        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
