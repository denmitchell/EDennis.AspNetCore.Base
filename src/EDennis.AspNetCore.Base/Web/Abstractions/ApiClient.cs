using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Web {
    //note: use ApiAttribute(ApiKey) to specify a key for the configuration file that
    //      differs from the class name
    public abstract class ApiClient : IApiClient {

        public HttpClient HttpClient { get; }
        public Api Api { get; }
        public ILogger Logger { get; }
        public IScopeProperties ScopeProperties { get; }

        public ApiClient(
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis,
            IScopeProperties scopeProperties,
            ILogger logger) {

            Logger = logger;
            ScopeProperties = scopeProperties;

            ApiKey = GetType().Name;
            HttpClient = httpClientFactory.CreateClient(ApiKey);
            try {
                Api = apis.CurrentValue[ApiKey];
            } catch (Exception ex) {
                Logger.LogError(ex, "For ApiClient {ApiClientType} Cannot find '{ApiKey}' in Apis section of Configuration", ApiKey);
            }

            BuildClient();

        }

        public virtual string ApiKey { get;}




        private void BuildClient() {

            #region BaseAddress
            if(!HttpClient.BaseAddress.ToString().Equals(Api.MainAddress))
                HttpClient.BaseAddress = new Uri(Api.MainAddress);
            #endregion
            #region DefaultRequestHeaders

            var headersToTransfer = Api.Mappings.HeadersToHeaders;
            var claimsToTransfer = Api.Mappings.ClaimsToHeaders;

            //add claims as headers
            if (ScopeProperties.Claims != null) {
                var claimsDictionary = ScopeProperties.Claims
                            .GroupBy(x => x.Type)
                            .ToDictionary(g => g.Key, g => new StringValues(g.Select(x => x.Value).ToArray()));

                foreach (var key in claimsToTransfer.Keys)
                    foreach (var claim in claimsDictionary.Where(d => d.Key == key))
                        HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, claim.Value.ToArray());
            }

            //add additional headers
            if (ScopeProperties.Headers != null) {
                foreach (var key in headersToTransfer.Keys)
                    foreach (var hdr in ScopeProperties.Headers.Where(d => d.Key == key))
                        HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, hdr.Value.ToArray());
            }

            #endregion

        }
    }
}
