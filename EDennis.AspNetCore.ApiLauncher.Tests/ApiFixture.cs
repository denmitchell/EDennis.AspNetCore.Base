using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web.Launcher.Tests {

    public class ExpectedResponses {
        public int Port { get; set; }
        public string ExpectedResponse { get; set; }
    }
    public class Urls {
        public int Port { get; set; }
        public string Url { get; set; }
    }

    public class ApiFixture : IDisposable {

        public List<ExpectedResponses> ExpectedResponses
            = new List<ExpectedResponses>();
        public List<Urls> Urls
            = new List<Urls>();

        private ApiLauncher launcher = new ApiLauncher();

        public ApiFixture() {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            launcher.StartApis(config);

            var apiSections = config.GetSection("ExpectedResponses").GetChildren();

            foreach (var apiSection in apiSections) {
                ExpectedResponses.Add(
                    new ExpectedResponses {
                        Port = int.Parse(apiSection.Key),
                        ExpectedResponse = apiSection.Value
                    });
            }


            apiSections = config.GetSection("Urls").GetChildren();

            foreach (var apiSection in apiSections) {
                Urls.Add(
                    new Urls {
                        Port = int.Parse(apiSection.Key),
                        Url = apiSection.Value
                    });
            }

        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    launcher.StopApis();
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
