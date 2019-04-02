using System;
using System.Net.Http;
using System.Linq;

namespace EDennis.AspNetCore.Base.Testing {
    public static class TestHttpClientFactory {

        public static HttpClient CreateReadonlyClient<TStartup>(
            ConfiguringWebApplicationFactory<TStartup> factory)
            where TStartup : class {

            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add(Interceptor.HDR_USE_READONLY, "");
            return httpClient;
        }


        public static HttpClient CreateWriteableClient<TStartup>(
            ConfiguringWebApplicationFactory<TStartup> factory)
            where TStartup : class {

            var instanceName = Guid.NewGuid().ToString();
            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add(Interceptor.HDR_USE_INMEMORY, instanceName);
            return httpClient;
        }

        public static string GetInstanceName(HttpClient httpClient) {
            var headers =httpClient.DefaultRequestHeaders.GetValues(Interceptor.HDR_USE_INMEMORY);
            var header = headers.FirstOrDefault();
            return header.ToString();
        }

    }

    public static class TestHttpClientFactoryExtensionMethods {

        public static string GetInstanceName(this HttpClient httpClient) {
            var headers = httpClient.DefaultRequestHeaders.GetValues(Interceptor.HDR_USE_INMEMORY);
            var header = headers.FirstOrDefault();
            return header.ToString();
        }

    }

}
