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

        public ApiClient(HttpClient httpClient, IConfiguration config,
            TestHeader testHeader) {
            HttpClient = httpClient;
            httpClient.DefaultRequestHeaders.Add(testHeader.Operation, testHeader.InstanceName);
            var baseAddress = config[$"Apis:{GetType().Name}:BaseAddress"];
            HttpClient.BaseAddress = new Uri(baseAddress);
        }
    }
}
