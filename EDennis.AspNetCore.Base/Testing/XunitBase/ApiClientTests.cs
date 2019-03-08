using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class ApiClientTests<TClient,TStartup> :
            IClassFixture<ApiLauncherFixture<TStartup>>, IDisposable
        where TStartup : class
        where TClient: ApiClient {
        protected readonly ApiLauncherFixture<TStartup> _fixture;

        protected ITestOutputHelper Output { get; }
        protected ScopeProperties ScopeProperties { get; }
        protected IConfiguration ApiConfiguration { get; }
        protected string InstanceName { get; }
        protected HttpClient HttpClient { get; }

        protected TClient ApiClient { get; }

        public ApiClientTests(ITestOutputHelper output,
                ApiLauncherFixture<TStartup> fixture,
                string testUser = "moe@stooges.org"
                ) {
            _fixture = fixture;
            Output = output;

            HttpClient = HttpClientFactory.Create();

            var port = fixture.Port;
            var baseAddress = $"http://localhost:{port}";

            var baseAddressConfig = new List<KeyValuePair<string, string>>();
            var clientName = typeof(TClient).Name;
            baseAddressConfig.Add(new KeyValuePair<string, string>($"Apis:{clientName}:BaseAddress", baseAddress));

            ApiConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(baseAddressConfig)
                .Build();

            InstanceName = Guid.NewGuid().ToString();
            ScopeProperties = new ScopeProperties {
                User = testUser
            };
            ScopeProperties.OtherProperties.Add(Interceptor.HDR_USE_INMEMORY, InstanceName);

            ApiClient = Activator.CreateInstance(typeof(TClient),
                    HttpClient, ApiConfiguration, ScopeProperties 
                ) as TClient;


            bool ping = HttpClient.Ping(5);
            if (ping == false)
                ping = HttpClient.Ping(5);

        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    HttpClient.SendResetAsync(Interceptor.HDR_DROP_INMEMORY, InstanceName);
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

