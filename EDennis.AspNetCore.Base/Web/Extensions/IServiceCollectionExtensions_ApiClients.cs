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
            where TClientInterface : class, IHasILogger
            where TClientImplementation : ApiClient, TClientInterface
             {

            services.AddDependencies<TClientImplementation>();
            services.AddApiClientInternal<TClientInterface, TClientImplementation>();

            return services;
        }



        public static IServiceCollection AddApiClient<TClient1>(this IServiceCollection services, bool traceable = true)
            where TClient1 : ApiClient {
            services.AddDependencies<TClient1>();
            services.AddApiClientInternal<TClient1, TClient1>(traceable);
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1>(this IServiceCollection services, bool traceable = true)
            where TClient1 : ApiClient => 
            services.AddApiClient<TClient1,TClient1>(traceable);

        public static IServiceCollection AddApiClients<TClient1, TClient2>(this IServiceCollection services, bool traceable = true)
            where TClient1 : ApiClient
            where TClient2 : ApiClient {
            services.AddDependencies<TClient1>();
            services.AddApiClientInternal<TClient1, TClient1>(traceable);
            services.AddApiClientInternal<TClient2, TClient2>(traceable);
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3>(this IServiceCollection services, bool traceable = true)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient {
            services.AddDependencies<TClient1>();
            services.AddApiClientInternal<TClient1, TClient1>(traceable);
            services.AddApiClientInternal<TClient2, TClient2>(traceable);
            services.AddApiClientInternal<TClient3, TClient3>(traceable);
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3, TClient4>(this IServiceCollection services, bool traceable = true)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient
            where TClient4 : ApiClient {
            services.AddDependencies<TClient1>();
            services.AddApiClientInternal<TClient1, TClient1>(traceable);
            services.AddApiClientInternal<TClient2, TClient2>(traceable);
            services.AddApiClientInternal<TClient3, TClient3>(traceable);
            services.AddApiClientInternal<TClient4, TClient4>(traceable);
            return services;
        }

        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3, TClient4, TClient5>(this IServiceCollection services, bool traceable = true)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient
            where TClient4 : ApiClient
            where TClient5 : ApiClient {
            services.AddDependencies<TClient1>();
            services.AddApiClientInternal<TClient1, TClient1>(traceable);
            services.AddApiClientInternal<TClient2, TClient2>(traceable);
            services.AddApiClientInternal<TClient3, TClient3>(traceable);
            services.AddApiClientInternal<TClient4, TClient4>(traceable);
            services.AddApiClientInternal<TClient5, TClient5>(traceable);
            return services;
        }


        private static void AddDependencies<TClientImplementation>(this IServiceCollection services)
            where TClientImplementation : class {

            services.TryAddScoped<ScopeProperties22, ScopeProperties22>();

            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TClientImplementation));

            if (isSecureClient) {
                services.TryAddSingleton<SecureTokenCache, SecureTokenCache>();
                services.TryAddScoped<IdentityServerApi>();
            }
        }

        private static IServiceCollection AddApiClientInternal<TClientInterface, TClientImplementation>(this IServiceCollection services, bool traceable = true)
            where TClientInterface : class, IHasILogger  
            where TClientImplementation : ApiClient, TClientInterface
            {


            if (traceable)
                services.AddScopedTraceable<TClientInterface, TClientImplementation>();
            else
                services.AddHttpClient<TClientInterface, TClientImplementation>();

            return services;
        }



    }
}