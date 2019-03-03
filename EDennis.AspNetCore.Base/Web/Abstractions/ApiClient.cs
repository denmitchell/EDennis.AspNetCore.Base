using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class ApiClient {

        public HttpClient HttpClient { get; set; }
        public ScopeProperties ScopeProperties { get; set; }
        public IConfiguration Configuration { get; set; }

        public ApiClient(HttpClient httpClient, IConfiguration config, ScopeProperties scopeProperties) {

            HttpClient = httpClient;
            ScopeProperties = scopeProperties;
            Configuration = config;

            //copy headers
            foreach(var prop in scopeProperties.OtherProperties.Where(x => x.Key.StartsWith(Interceptor.HDR_PREFIX))) {
                httpClient.DefaultRequestHeaders.Add(prop.Key, prop.Value.ToString());
            }

            var baseAddress = config[$"Apis:{GetType().Name}:BaseAddress"];
            HttpClient.BaseAddress = new Uri(baseAddress);
        }
    }
}
