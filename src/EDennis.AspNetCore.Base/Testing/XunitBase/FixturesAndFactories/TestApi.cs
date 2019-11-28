using EDennis.Samples.SharedModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EDennis.AspNetCore.Base.Testing {

    public class TestApi<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class {

        private readonly Dictionary<string, Func<HttpClient>> _create;
        private readonly string _instanceName;

        public TestApi(Dictionary<string, Func<HttpClient>> create,
            Dictionary<string, Action> dispose, string httpClientName,
            string instanceName) {
            _create = create;

            create.Add(httpClientName, CreateClient);
            dispose.Add(httpClientName, Dispose);

            _instanceName = instanceName;

            var _ = Server; //ensure creation;
        }



        protected override void ConfigureWebHost(IWebHostBuilder builder) {

            builder.ConfigureServices(services => {

                services.AddScoped<IHttpClientFactory>(provider => {
                    return new TestHttpClientFactory(_create);
                });
                foreach(var apiKey in _create.Keys)
                    services.AddHttpClient(apiKey, configure=> { 
                        configure.DefaultRequestHeaders.Add(Constants.TESTING_INSTANCE_KEY, _instanceName);
                    });

                services.AddControllers();
            });

        }


    }


}
