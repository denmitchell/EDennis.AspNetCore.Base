using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
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


        private static void AddDependencies<TClientImplementation>(this IServiceCollection services)
            where TClientImplementation : class {

            services.TryAddScoped<IScopeProperties, ScopeProperties>();

            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TClientImplementation));

            if (isSecureClient) {
                services.TryAddSingleton<SecureTokenService, SecureTokenService>();
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