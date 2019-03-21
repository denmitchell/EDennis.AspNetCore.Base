using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
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
            foreach (var prop in scopeProperties.OtherProperties.Where(x => x.Key == HEADER_KEY)) {
                var headers = prop.Value as Dictionary<string, StringValues>;
                foreach (var header in headers)
                    foreach(var value in headers.Values)
                        httpClient.DefaultRequestHeaders.Add(header.Key, value.ToString());
            }

            //copy ClientTrace from ClientTrace entry in scopeProperties
            foreach (var prop in scopeProperties.OtherProperties.Where(x => x.Key == "ClientTrace")) {
                var headers = prop.Value as Dictionary<string, StringValues>;
                foreach (var header in headers)
                    foreach (var value in headers.Values)
                        httpClient.DefaultRequestHeaders.Add(header.Key, value.ToString());
            }

            var baseAddress = config[$"Apis:{GetType().Name}:BaseAddress"];
            HttpClient.BaseAddress = new Uri(baseAddress);
        }
    }
}
