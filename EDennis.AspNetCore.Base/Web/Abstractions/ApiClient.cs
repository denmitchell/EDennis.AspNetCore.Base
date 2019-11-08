using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base.Web
{
    //note: use ApiAttribute(ApiKey) to specify a key for the configuration file that
    //      differs from the class name
    public abstract class ApiClient : IHasILogger {

        public HttpClient HttpClient { get; set; }
        public Api Api { get; set; }
        public ILogger Logger { get; }
        public IScopeProperties ScopeProperties { get; set; }

        public ApiClient(HttpClient httpClient,
            IOptionsMonitor<AppSettings> appSettings, 
            IScopeProperties scopeProperties,
            ILogger logger) {

            HttpClient = httpClient;
            try {
                Api = appSettings.CurrentValue.Apis[GetApiKey()];
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
            string baseAddress = null;


            if (Api.ApiLauncher) {
                baseAddress = $"{Api.Scheme}://{Api.Host}:{Api.HttpsPort}";
            }

            HttpClient.BaseAddress = new Uri(baseAddress);
            #endregion
            #region DefaultRequestHeaders

            //create headers from list
            ScopeProperties.Headers
                .ToList()
                .ForEach(x => HttpClient.DefaultRequestHeaders
                .AddOrReplace(x.Key, x.Value.ToArray()));


            #endregion

        }
    }
}
