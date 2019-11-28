using EDennis.AspNetCore.Base.Web;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Testing {

    public class TestHttpClientFactory : IHttpClientFactory {

        private readonly Dictionary<string, Func<HttpClient>> _factory;

        public TestHttpClientFactory(Dictionary<string, Func<HttpClient>> factory) {
            _factory = factory;
        }


        public HttpClient CreateClient(string name) {
            return _factory[name]();
        }
    }
}
