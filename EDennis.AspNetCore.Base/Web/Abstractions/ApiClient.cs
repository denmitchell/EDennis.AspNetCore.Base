using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Web
{
    public abstract class ApiClient {

        public const string HEADER_KEY = "ApiClientHeaders";

        public HttpClient HttpClient { get; set; }
        public ScopeProperties ScopeProperties { get; set; }
        public IConfiguration Configuration { get; set; }

        public ApiClient(HttpClient httpClient, IConfiguration config, ScopeProperties scopeProperties) {

            HttpClient = httpClient;
            ScopeProperties = scopeProperties;
            Configuration = config;

            //build headers from ApiClientHeaders entry in scopeProperties
            foreach (var targetProp in scopeProperties.OtherProperties.Where(x => x.Key == this.GetType().Name)) {
                var dict = targetProp.Value as Dictionary<string, object>;
                foreach (var prop in dict) {
                    var headers = prop.Value as List<KeyValuePair<string, StringValues>>;
                    foreach (var header in headers)
                        foreach (var value in header.Value)
                            httpClient.DefaultRequestHeaders.Add(header.Key, value.ToString());
                }
            }

            //add X-User header from ScopeProperties, if the header doesn't already exist.
            var hdrs = httpClient.DefaultRequestHeaders;
            var userHeaderCount = hdrs.Where(x => x.Key == "X-User").Count();
            if (userHeaderCount == 0 && !string.IsNullOrWhiteSpace(ScopeProperties.User))
                hdrs.Add("X-User", ScopeProperties.User);


            var baseAddress = config[$"Apis:{GetType().Name}:BaseAddress"];
            var env = config["ASPNETCORE_ENVIRONMENT"];

            if (string.IsNullOrEmpty(baseAddress))
                throw new ApplicationException($"Cannot find entry for 'Apis:{GetType().Name}:BaseAddress' in the configuration (e.g., appsettings.{env}.json).  Each ApiClient and SecureApiClient must have an entry in the configuration that corresponds to the class name of the ApiClient or SecureApiClient.");


            HttpClient.BaseAddress = new Uri(baseAddress);
        }
    }
}
