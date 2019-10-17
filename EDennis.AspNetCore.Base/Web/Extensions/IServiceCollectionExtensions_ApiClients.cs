using Castle.Core.Configuration;
using Castle.DynamicProxy;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Web.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Web.Extensions {
    public static class IServiceCollectionExtensions_ApiClients {


        public static IServiceCollection AddApiClient<TApiClientImplementation>(this IServiceCollection services)
            where TApiClientImplementation : ApiClient {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TApiClientImplementation>();
            return services;
        }


        public static IServiceCollection AddApiClient<TApiClientInterface, TApiClientImplementation>(this IServiceCollection services)
            where TApiClientInterface : class
            where TApiClientImplementation : ApiClient, TApiClientInterface {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TApiClientInterface, TApiClientImplementation>();
            return services;
        }



        private static IServiceCollection AddScoped<TApiClientInterface, TApiClientImplementation>(this IServiceCollection services)
            where TApiClientInterface : class
            where TApiClientImplementation : ApiClient, TApiClientInterface {

            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TApiClientImplementation));


            services.AddHttpClient(); //this ensures that all related services are created

            if (isSecureClient) {
                services.TryAddSingleton<SecureTokenCache, SecureTokenCache>();
                services.TryAddScoped<IdentityServerApi>();
            }

            services = services.AddScoped<TApiClientInterface>(f => {
                var loggers = f.GetRequiredService<IEnumerable<ILogger<TApiClientImplementation>>>();
                var configuration = f.GetRequiredService<IConfiguration>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                SecureTokenCache secureTokenCache = null;
                IWebHostEnvironment webHostEnvironment = null;
                IdentityServerApi identityServerApi = null;
                if (isSecureClient) {
                    secureTokenCache = f.GetRequiredService<SecureTokenCache>();
                    webHostEnvironment = f.GetRequiredService<IWebHostEnvironment>();
                    identityServerApi = f.GetRequiredService<IdentityServerApi>();
                }
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);
                var httpClientFactory = f.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(typeof(ApiClient).Name);

                if (isSecureClient) {
                    var api = (TApiClientImplementation)new ProxyGenerator()
                        .CreateClassProxy(typeof(TApiClientImplementation),
                            new object[] { httpClient, configuration, scopeProperties, identityServerApi,
                            secureTokenCache, webHostEnvironment, activeLogger},
                            new TraceInterceptor(activeLogger, scopeProperties));
                    return api;
                } else {
                    var api = (TApiClientImplementation)new ProxyGenerator()
                        .CreateClassProxy(typeof(TApiClientImplementation),
                            new object[] { httpClient, configuration, scopeProperties, activeLogger },
                            new TraceInterceptor(activeLogger, scopeProperties));
                    return api;
                }
            });

            return services;

        }




        private static IServiceCollection AddScoped<TApiClientImplementation>(this IServiceCollection services)
            where TApiClientImplementation : ApiClient {

            services.AddHttpClient(); //this ensures that all related services are created


            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TApiClientImplementation));

            if (isSecureClient) {
                services.TryAddSingleton<SecureTokenCache, SecureTokenCache>();
                services.TryAddScoped<IdentityServerApi>();
            }

            services = services.AddScoped(f => {
                var loggers = f.GetRequiredService<IEnumerable<ILogger<TApiClientImplementation>>>();
                var configuration = f.GetRequiredService<IConfiguration>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                SecureTokenCache secureTokenCache = null;
                IWebHostEnvironment webHostEnvironment = null;
                IdentityServerApi identityServerApi = null;
                if (isSecureClient) {
                    secureTokenCache = f.GetRequiredService<SecureTokenCache>();
                    webHostEnvironment = f.GetRequiredService<IWebHostEnvironment>();
                    identityServerApi = f.GetRequiredService<IdentityServerApi>();
                }
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);
                var httpClientFactory = f.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(typeof(ApiClient).Name);

                if (isSecureClient) {
                    var api = (TApiClientImplementation)new ProxyGenerator()
                        .CreateClassProxy(typeof(TApiClientImplementation),
                            new object[] { httpClient, configuration, scopeProperties, identityServerApi,
                            secureTokenCache, webHostEnvironment, activeLogger},
                            new TraceInterceptor(activeLogger, scopeProperties));
                    return api;
                } else {
                    var api = (TApiClientImplementation)new ProxyGenerator()
                        .CreateClassProxy(typeof(TApiClientImplementation),
                            new object[] { httpClient, configuration, scopeProperties, activeLogger },
                            new TraceInterceptor(activeLogger, scopeProperties));
                    return api;
                }
            });

            return services;

        }



        [Obsolete("Use AddApiClient instead")]
        public static IServiceCollection AddApiClients<TClient1>(this IServiceCollection services)
        where TClient1 : ApiClient {
            services.AddApiClient<TClient1>();
            return services;
        }

        [Obsolete("Use AddApiClient instead")]
        public static IServiceCollection AddApiClients<TClient1, TClient2>(this IServiceCollection services)
            where TClient1 : ApiClient
            where TClient2 : ApiClient {
            services.AddApiClient<TClient1>();
            services.AddApiClient<TClient2>();
            return services;
        }

        [Obsolete("Use AddApiClient instead")]
        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3>(this IServiceCollection services)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient {
            services.AddApiClient<TClient1>();
            services.AddApiClient<TClient2>();
            services.AddApiClient<TClient3>();
            return services;
        }

        [Obsolete("Use AddApiClient instead")]
        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3, TClient4>(this IServiceCollection services)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient
            where TClient4 : ApiClient {
            services.AddApiClient<TClient1>();
            services.AddApiClient<TClient2>();
            services.AddApiClient<TClient3>();
            services.AddApiClient<TClient4>();
            return services;
        }

        [Obsolete("Use AddApiClient instead")]
        public static IServiceCollection AddApiClients<TClient1, TClient2, TClient3, TClient4, TClient5>(this IServiceCollection services)
            where TClient1 : ApiClient
            where TClient2 : ApiClient
            where TClient3 : ApiClient
            where TClient4 : ApiClient
            where TClient5 : ApiClient {
            services.AddApiClient<TClient1>();
            services.AddApiClient<TClient2>();
            services.AddApiClient<TClient3>();
            services.AddApiClient<TClient4>();
            services.AddApiClient<TClient5>();
            return services;
        }
    }
}