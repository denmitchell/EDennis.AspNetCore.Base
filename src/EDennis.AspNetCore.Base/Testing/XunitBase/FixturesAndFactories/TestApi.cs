using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Collections.Generic;
using System;

namespace EDennis.AspNetCore.Base.Testing {

    public class TestApi<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class {

        private readonly Dictionary<string, Func<HttpClient>> _create;
        private readonly string _instanceName;
        private readonly string _environmentName;

        public TestApi(Dictionary<string, Func<HttpClient>> create,
            Dictionary<string, Action> dispose, string httpClientName,
            string instanceName, string environmentName = null) {
            _create = create;

            create.Add(httpClientName, CreateClient);
            dispose.Add(httpClientName, Dispose);

            _instanceName = instanceName;
            _environmentName = environmentName;

            var _ = Server; //ensure creation;
        }



        protected override void ConfigureWebHost(IWebHostBuilder builder) {

            if (_environmentName != null)
                builder.UseEnvironment(_environmentName);

            builder.ConfigureServices(services => {

                services.AddSingleton<IHttpClientFactory>(provider => {
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
