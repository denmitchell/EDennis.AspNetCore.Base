using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class ReadonlyApiClientTests<TClient,TStartup> : IDisposable
        where TStartup : class
        where TClient: ApiClient {

        protected ITestOutputHelper Output { get; }
        protected string InstanceName { get; }

        protected TClient ApiClient { get; }

        public ReadonlyApiClientTests(ITestOutputHelper output,
                ApiLauncherFactory<TStartup> factory) {

            Output = output;
            ApiClient = TestApiClientFactory.CreateReadonlyClient<TClient, TStartup>(factory);
            InstanceName = ApiClient.GetInstanceName();

        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                }
                disposedValue = true;
            }
        }


        public void Dispose() {
            Dispose(true);
        }
        #endregion




    }
}

