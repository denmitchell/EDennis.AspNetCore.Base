using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class ApiClient {

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

            var baseAddress = config[$"Apis:{GetType().Name}:BaseAddress"];
            HttpClient.BaseAddress = new Uri(baseAddress);
        }
    }
}
