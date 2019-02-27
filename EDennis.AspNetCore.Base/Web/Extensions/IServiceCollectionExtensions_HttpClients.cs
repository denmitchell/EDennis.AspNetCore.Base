using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EDennis.AspNetCore.Base.Web.Extensions {
    public static class IServiceCollectionExtensions_HttpClient {


        public static IServiceCollection AddApiClients<TClient1>(this IServiceCollection services)
            where TClient1 : ApiClient {
            services.AddScoped<ScopeProperties>();
            services.AddHttpClient<TClient1>();
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2>(this IServiceCollection services)
            where TClient1 : ApiClient
            where TClient2 : ApiClient {
            services.AddApiClients<TClient1>();
            services.AddHttpClient<TClient2>();
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3>(this IServiceCollection services)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient {
            services.AddApiClients<TClient1, TClient2>();
            services.AddHttpClient<TClient3>();
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3, TClient4>(this IServiceCollection services)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient
            where TClient4 : ApiClient {
            services.AddApiClients<TClient1, TClient2, TClient3>();
            services.AddHttpClient<TClient4>();
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3, TClient4, TClient5>(this IServiceCollection services)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient
            where TClient4 : ApiClient
            where TClient5 : ApiClient {
            services.AddApiClients<TClient1, TClient2, TClient3, TClient4>();
            services.AddHttpClient<TClient5>();
            return services;
        }
    }
}