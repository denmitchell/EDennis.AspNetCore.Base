using Castle.Core.Configuration;
using Castle.DynamicProxy;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Web.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EDennis.AspNetCore.Base.Web.Extensions {
    public static class IServiceCollectionExtensions_ApiClients {


        public static IServiceCollection AddApiClient<TClientInterface, TClientImplementation>(this IServiceCollection services, bool traceable = true)
            where TClientImplementation : class, TClientInterface
            where TClientInterface : class {
            services.TryAddScoped<ScopeProperties, ScopeProperties>();

            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TClientImplementation));

            if (isSecureClient) {
                services.TryAddSingleton<SecureTokenCache, SecureTokenCache>();
                services.TryAddScoped<IdentityServerApi>();
            }

            if (traceable)
                services.AddScopedTraceable<TClientInterface, TClientImplementation>();
            else
                services.AddHttpClient<TClientInterface, TClientImplementation>();

            return services;
        }


        public static IServiceCollection AddApiClients<TClient1>(this IServiceCollection services, bool traceable)
            where TClient1 : ApiClient {
            services.AddApiClient<TClient1,TClient1>(traceable);
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2>(this IServiceCollection services, bool traceable)
            where TClient1 : ApiClient
            where TClient2 : ApiClient {
            services.AddApiClient<TClient1, TClient1>(traceable);
            services.AddApiClient<TClient2, TClient2>(traceable);
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3>(this IServiceCollection services, bool traceable)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient {
            services.AddApiClient<TClient1, TClient1>(traceable);
            services.AddApiClient<TClient2, TClient2>(traceable);
            services.AddApiClient<TClient3, TClient3>(traceable);
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3, TClient4>(this IServiceCollection services, bool traceable)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient
            where TClient4 : ApiClient {
            services.AddApiClient<TClient1, TClient1>(traceable);
            services.AddApiClient<TClient2, TClient2>(traceable);
            services.AddApiClient<TClient3, TClient3>(traceable);
            services.AddApiClient<TClient4, TClient4>(traceable);
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3, TClient4, TClient5>(this IServiceCollection services, bool traceable)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient
            where TClient4 : ApiClient
            where TClient5 : ApiClient {
            services.AddApiClient<TClient1, TClient1>(traceable);
            services.AddApiClient<TClient2, TClient2>(traceable);
            services.AddApiClient<TClient3, TClient3>(traceable);
            services.AddApiClient<TClient4, TClient4>(traceable);
            services.AddApiClient<TClient5, TClient5>(traceable);
            return services;
        }




    }
}