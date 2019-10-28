using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        public IScopeProperties ScopeProperties { get; set; }
        public ILogger Logger { get; }

        public ApiClient(HttpClient httpClient,
            IScopeProperties scopeProperties, ILogger logger) {

            HttpClient = httpClient;
            ScopeProperties = scopeProperties;
            Logger = logger;

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
            Api api = null;

            try {
                api = ScopeProperties.ActiveProfile.Apis[ApiKey];
            } catch {
                var ex = new ApplicationException($"Api key {ApiKey} missing in profile {ScopeProperties.ActiveProfile.Name}");
                Logger.LogError(ex, ex.Message);
            }

            if (api.Embedded) {
                baseAddress = $"{ScopeProperties.Scheme}://{ScopeProperties.Host}:{ScopeProperties.Port}/{ApiKey}";
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
