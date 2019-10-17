using EDennis.AspNetCore.Base.Web.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EDennis.AspNetCore.Base.Web.Extensions
{
    public static class IServiceCollectionExtensions_ApiClients {



        public static IServiceCollection AddRepo<TRepoImplementation, TContext>(this IServiceCollection services)
            where TRepoImplementation : class, IRepo
            where TContext : DbContext {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TRepoImplementation, TContext>();
            return services;
        }


        public static IServiceCollection AddRepo<TRepoInterface, TRepoImplementation, TContext>(this IServiceCollection services)
            where TRepoInterface : class
            where TRepoImplementation : class, IRepo, TRepoInterface
            where TContext : DbContext {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TRepoInterface, TRepoImplementation, TContext>();
            return services;
        }


        /*
    public SecureApiClient(HttpClient httpClient,
        IConfiguration config,
        ScopeProperties scopeProperties,
        ApiClient identityServerApiClient,
        SecureTokenCache secureTokenCache,
        IWebHostEnvironment hostingEnvironment)
        : base(httpClient, config, scopeProperties) {

        */



        private static IServiceCollection AddScoped<TSecureApiClient, TContext>(this IServiceCollection services)
            where TRepoImplementation : class
            where TContext : DbContext {
            return services.AddScoped(f => {
                var loggers = f.GetRequiredService<IEnumerable<ILogger<TRepoImplementation>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);
                var context = f.GetRequiredService<TContext>();
                var repo = (TRepoImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TRepoImplementation),
                        new object[] { context, scopeProperties, activeLogger },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }

        private static IServiceCollection AddScoped<TRepoInterface, TRepoImplementation, TContext>(this IServiceCollection services)
            where TRepoInterface : class
            where TRepoImplementation : class, TRepoInterface
            where TContext : DbContext {
            return services.AddScoped<TRepoInterface>(f => {
                var loggers = f.GetRequiredService<IEnumerable<ILogger<TRepoInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);
                var context = f.GetRequiredService<TContext>();
                var repo = (TRepoImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TRepoImplementation),
                        new object[] { context, scopeProperties, activeLogger },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }

        public static IServiceCollection AddApiClient<TClientInterface, TClientImplementation>(this IServiceCollection services)
            where TClientImplementation : class, TClientInterface
            where TClientInterface : class {
            services.TryAddScoped<ScopeProperties, ScopeProperties>();
            if (typeof(ApiClient).IsAssignableFrom(typeof(TClientImplementation))) { 
                services.TryAddSingleton<SecureTokenCache, SecureTokenCache>();
                services.AddHttpClient<TClientInterface, TClientImplementation>();
            } else {
                services.AddScoped<TClientInterface, TClientImplementation>();
            }
            return services;
        }


        public static IServiceCollection AddApiClients<TClient1>(this IServiceCollection services)
            where TClient1 : ApiClient {
            services.TryAddSingleton<SecureTokenCache,SecureTokenCache>();
            services.TryAddScoped<ScopeProperties,ScopeProperties>();
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