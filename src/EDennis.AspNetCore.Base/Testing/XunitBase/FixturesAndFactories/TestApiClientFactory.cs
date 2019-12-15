using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Testing.XunitBase.FixturesAndFactories;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Testing {
    public static class TestApiClientFactory {

        public const int TIMEOUT_SECONDS = 5;
        public static ConcurrentDictionary<Type, HttpClient> HttpClients = new ConcurrentDictionary<Type, HttpClient>();


        public static TClient CreateApiClient<TClient>()
            where TClient : ApiClient
            => CreateApiClient<TClient>(TestScopePropertiesFactory.GetScopeProperties(),
                new ConfigurationFixture().Configuration, typeof(TClient).Name);

        public static TClient CreateApiClient<TClient>(string userName, string role)
            where TClient : ApiClient
            => CreateApiClient<TClient>(TestScopePropertiesFactory.GetScopeProperties(userName, role),
                new ConfigurationFixture().Configuration, typeof(TClient).Name);

        public static TClient CreateApiClient<TClient>(IScopeProperties scopeProperties)
            where TClient : ApiClient
            => CreateApiClient<TClient>(scopeProperties, new ConfigurationFixture().Configuration, typeof(TClient).Name);

        public static TClient CreateApiClient<TClient>(
            IScopeProperties scopeProperties, IConfiguration Configuration, string apisConfigKey)
            where TClient : ApiClient {

            var apis = new Apis();
            Configuration.GetSection(apisConfigKey).Bind(apis);
            var iomApis = new TestOptionsMonitor<Apis>(apis);

            var httpClient = HttpClients.GetOrAdd(typeof(TClient), t => {
                var httpClient = new HttpClient();
                HttpClients.TryAdd(t, httpClient);
                return httpClient;
            });

            var baseAddress = httpClient.BaseAddress.ToString();

            var client = (TClient)Activator.CreateInstance(typeof(TClient),
                new object[] { httpClient, iomApis, scopeProperties, NullLogger.Instance, new NullScopedLogger() });

            var baseAddressAfter = client.HttpClient.BaseAddress.ToString();

            if (baseAddressAfter != baseAddress)
                httpClient.BaseAddress = client.HttpClient.BaseAddress;

            bool ping = httpClient.Ping(TIMEOUT_SECONDS);
            if (ping == false)
                ping = httpClient.Ping(TIMEOUT_SECONDS);

            return client;
        }



        public static TClient CreateSecureApiClient<TClient>()
            where TClient : SecureApiClient
            => CreateSecureApiClient<TClient>(TestScopePropertiesFactory.GetScopeProperties(),
                new ConfigurationFixture().Configuration, typeof(TClient).Name);

        public static TClient CreateSecureApiClient<TClient>(string userName, string role)
            where TClient : SecureApiClient
            => CreateSecureApiClient<TClient>(TestScopePropertiesFactory.GetScopeProperties(userName, role),
                new ConfigurationFixture().Configuration, typeof(TClient).Name);


        public static TClient CreateSecureApiClient<TClient>(
            IScopeProperties scopeProperties, IConfiguration Configuration, string apisConfigKey)

            where TClient : SecureApiClient {

            throw new NotImplementedException();

            //var apis = new Apis();
            //Configuration.GetSection(apisConfigKey).Bind(apis);
            //var iomApis = new TestOptionsMonitor<Apis>(apis);

            //var httpClient = HttpClients.GetOrAdd(typeof(TClient), t => {
            //    var httpClient = new HttpClient();
            //    HttpClients.TryAdd(t, httpClient);
            //    return httpClient;
            //});

            //var httpClientFactory = new TestHttpClientFactory(TestApis.CreateClient);

            //var secureTokenService = new SecureTokenService(httpClientFactory, iomApis, NullLogger<SecureTokenService>.Instance, null) {
            //    ApplicationName = typeof(TClient).Assembly.GetName().Name
            //};

            //var baseAddress = httpClient.BaseAddress.ToString();

            //var client = (TClient)Activator.CreateInstance(typeof(TClient),
            //    new object[] { httpClient, iomApis, scopeProperties, secureTokenService, NullLogger.Instance, new NullScopedLogger() });

            //var baseAddressAfter = client.HttpClient.BaseAddress.ToString();

            //if (baseAddressAfter != baseAddress)
            //    httpClient.BaseAddress = client.HttpClient.BaseAddress;

            //bool ping = httpClient.Ping(TIMEOUT_SECONDS);
            //if (ping == false)
            //    ping = httpClient.Ping(TIMEOUT_SECONDS);

            //return client;

        }
    }
}
