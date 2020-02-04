using Colors2.Models;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Colors2ExternalApi.Lib.ApiClients {
    public class Colors2Api : ApiClient,
        IRepoControllerApiClient<Rgb>,
        IQueryControllerApiClient<Hsl>,
        ISqlServerControllerApiClient<Color2DbContext> {

        public Colors2Api(IHttpClientFactory httpClientFactory, IOptionsMonitor<Apis> apis, IScopeProperties scopeProperties, IWebHostEnvironment env) : base(httpClientFactory, apis, scopeProperties, env) {
        }

        //to access conflicting extension methods...
        public IRepoControllerApiClient<Rgb> Rgb { get => this; }
        public IQueryControllerApiClient<Hsl> Hsl { get => this; }

        public string GetControllerUrl(Type type) {
            if (type == typeof(Rgb))
                return "api/Rgb";
            else if (type == typeof(Hsl))
                return "api/Hsl";
            else if (type == typeof(Color2DbContext))
                return "api/Proc";
            else
                return null;
        }
    }
}
