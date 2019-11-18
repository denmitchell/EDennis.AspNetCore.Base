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
    public abstract class ApiClient {

        public HttpClient HttpClient { get; set; }
        public Api Api { get; set; }
        public ILogger Logger { get; }
        public IScopeProperties ScopeProperties { get; set; }

        public ApiClient(HttpClient httpClient,
            IOptionsMonitor<Apis> apis, 
            IScopeProperties scopeProperties,
            ILogger logger) {

            HttpClient = httpClient;
            try {
                Api = apis.CurrentValue[GetApiKey()];
            } catch (Exception ex) {
                Logger.LogError(ex, "For ApiClient {ApiClientType} Cannot find '{ApiKey}' in Apis section of Configuration", this.GetType().Name, GetApiKey());
            }
            Logger = logger;

            ScopeProperties = scopeProperties;

            BuildClient();

        }

        public virtual string ApiKey { get; set; } = GetApiKey();


        private static string GetApiKey() {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            var attr =(ApiAttribute)Attribute.GetCustomAttribute(type, typeof(ApiAttribute));
            if (attr != null)
                return attr.Key;
            else
                return type.Name;
        }


        private void BuildClient() {

            #region BaseAddress
            HttpClient.BaseAddress = new Uri(Api.MainAddress);
            #endregion
            #region DefaultRequestHeaders

            var headersToTransfer = Api.Mappings.HeadersToHeaders;
            var claimsToTransfer = Api.Mappings.ClaimsToHeaders;

            var claimsDictionary = ScopeProperties.Claims
                        .GroupBy(x => x.Type)
                        .ToDictionary(g => g.Key, g => new StringValues(g.Select(x => x.Value).ToArray()));

            //add claims as headers
            foreach (var key in claimsToTransfer.Keys)
                foreach(var claim in claimsDictionary.Where(d=>d.Key == key))
                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, claim.Value.ToArray());

            //add additional headers
            foreach (var key in headersToTransfer.Keys)
                foreach(var hdr in ScopeProperties.Headers.Where(d=>d.Key == key))
                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, hdr.Value.ToArray());


            #endregion

        }
    }
}
