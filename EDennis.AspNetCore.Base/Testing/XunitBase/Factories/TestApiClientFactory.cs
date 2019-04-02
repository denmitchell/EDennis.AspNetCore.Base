using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public static class TestApiClientFactory {

        public const int TIMEOUT_SECONDS = 5;


        public static TClient CreateReadonlyClient<TClient, TStartup>(
            ApiLauncherFactory<TStartup> fixture)
            where TClient : ApiClient
            where TStartup : class {

            var httpClient = HttpClientFactory.Create();

            var port = fixture.Port;
            var baseAddress = $"http://localhost:{port}";

            var baseAddressConfig = new List<KeyValuePair<string, string>>();
            var clientName = typeof(TClient).Name;
            baseAddressConfig.Add(new KeyValuePair<string, string>($"Apis:{clientName}:BaseAddress", baseAddress));

            var apiConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(baseAddressConfig)
                .Build();

            var scopeProperties = new ScopeProperties { };

            var apiClientHeaders = new List<KeyValuePair<string, StringValues>> {
                new KeyValuePair<string, StringValues>(
                    Interceptor.HDR_USE_READONLY, "")
            };

            var dict = new Dictionary<string, object> {
                { Web.ApiClient.HEADER_KEY, apiClientHeaders }
            };

            scopeProperties.OtherProperties.Add(
                typeof(TClient).Name, dict);


            var apiClient = Activator.CreateInstance(typeof(TClient),
                    httpClient, apiConfiguration, scopeProperties
                ) as TClient;


            bool ping = httpClient.Ping(TIMEOUT_SECONDS);
            if (ping == false)
                ping = httpClient.Ping(TIMEOUT_SECONDS);

            return apiClient;

        }



        public static TClient CreateWriteableClient<TClient, TStartup>(
            ApiLauncherFactory<TStartup> fixture, string testUser = TestRepoFactory.DEFAULT_USER)
            where TClient : ApiClient
            where TStartup : class {

            var httpClient = HttpClientFactory.Create();

            var port = fixture.Port;
            var baseAddress = $"http://localhost:{port}";

            var baseAddressConfig = new List<KeyValuePair<string, string>>();
            var clientName = typeof(TClient).Name;
            baseAddressConfig.Add(new KeyValuePair<string, string>($"Apis:{clientName}:BaseAddress", baseAddress));

            var apiConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(baseAddressConfig)
                .Build();

            var instanceName = Guid.NewGuid().ToString();
            var scopeProperties = new ScopeProperties {
                User = testUser
            };

            var apiClientHeaders = new List<KeyValuePair<string, StringValues>> {
                new KeyValuePair<string, StringValues>(
                    Interceptor.HDR_USE_INMEMORY, instanceName)
            };

            var dict = new Dictionary<string, object> {
                { Web.ApiClient.HEADER_KEY, apiClientHeaders }
            };

            scopeProperties.OtherProperties.Add(
                typeof(TClient).Name, dict);


            var apiClient = Activator.CreateInstance(typeof(TClient),
                    httpClient, apiConfiguration, scopeProperties
                ) as TClient;


            bool ping = httpClient.Ping(TIMEOUT_SECONDS);
            if (ping == false)
                ping = httpClient.Ping(TIMEOUT_SECONDS);

            return apiClient;

        }


        public static string GetInstanceName(ApiClient apiClient) {
            var httpClient = apiClient.HttpClient;
            var headers = httpClient.DefaultRequestHeaders.GetValues(Interceptor.HDR_USE_INMEMORY);
            var header = headers.FirstOrDefault();
            return header.ToString();
        }

    }

    public static class TestApiClientFactoryExtensionMethods {

        public static string GetInstanceName(this ApiClient apiClient) {
            var httpClient = apiClient.HttpClient;
            var headers = httpClient.DefaultRequestHeaders.GetValues(Interceptor.HDR_USE_INMEMORY);
            var header = headers.FirstOrDefault();
            return header.ToString();
        }

    }

}

