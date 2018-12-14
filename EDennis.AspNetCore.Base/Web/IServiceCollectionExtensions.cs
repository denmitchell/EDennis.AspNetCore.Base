using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// Extensions to IServiceCollection that simplify setup of
    /// dependency injection, based upon configuration data, for
    /// (a) DbContextPools of DbContextBase classes
    ///     (reflectively invokes AddDbContextPool<SomeDbContextBaseClass>)
    /// (b) Repositories using DbContextBase classes
    ///     (reflectively invokes AddTransient<SomeRepoClass,SomeRepoClass>)
    /// (c) Services using HttpClient (service proxies).
    ///     (reflectively invokes AddHttpClient<SomeServiceClass>)
    /// 
    /// NOTE: These extensions use reflection to identify
    /// all relevant classes and to build the normal 
    /// extension methods identified above.
    /// </summary>
    public static class IServiceCollectionExtensions {

        /// <summary>
        /// Configures dependency injection (DI) for all classes
        /// implementing IProxy (having an HttpClient property).
        /// The configuration uses the standard DI extension 
        /// method AddHttpClient (which uses HttpClientFactory 
        /// behind the scenes to build typed HttpClients)
        /// </summary>
        /// <param name="services">a reference to IServiceCollection</param>
        /// <param name="config">a reference to the configuration</param>
        public static void AddHttpClients(this IServiceCollection services,
            IConfiguration config) {

            // get an enumeration of all IProxy classes
            var apiProxyTypes = GetIApiProxyTypes();

            //get information on all Apis, for which there should be service proxies 
            var apiConfigs = config.GetSection("Apis").GetChildren();

            //set a reference to the AddHttpClient method
            var addHttpClientMethod = GetAddHttpClientMethod();

            //iterate over all IApiProxy classes 
            foreach (var apiProxyType in apiProxyTypes) {

                //get the relevant API configuration
                var apiConfig = apiConfigs
                    .Where(x => x.Key == apiProxyType.Name).FirstOrDefault();

                //get the base address URL
                var baseAddress = apiConfig["BaseAddress"];

                //get a reference to the MethodInfo for AddHttpClient
                MethodInfo addHttpClientMethodGeneric = 
                    addHttpClientMethod.MakeGenericMethod(apiProxyType);

                //build the options for the HttpClient object
                Action<HttpClient> options = opt => {
                    opt.BaseAddress = new Uri(baseAddress);
                };

                //invoke the dependency injection method
                addHttpClientMethodGeneric.Invoke(services, 
                    new object[] { services, options });
            }

        }

        /// <summary>
        /// Configures dependency injection (DI) for all classes
        /// extending DbContextBase.
        /// The configuration uses the standard DI extension 
        /// method AddDbContextPool (which creates singleton 
        /// instances of DbContextOptions and a pool of 
        /// DbContextBase objects for each connection)
        /// </summary>
        /// <param name="services">a reference to IServiceCollection</param>
        /// <param name="config">a reference to the configuration</param>
        /// <param name="poolSize">The number of DbContextBase 
        /// instances per pool</param>
        public static void AddDbContextPools(this IServiceCollection services,
            IConfiguration config, int poolSize = 128) {

            // get an enumeration of all DbContextBase classes
            var dbContextBaseTypes = GetDbContextBaseTypes();

            //get information on all Connection Strings 
            var dbContextBaseConfigs = config.GetSection("ConnectionStrings").GetChildren();

            //set a reference to the AddDbContextPool method
            var addDbContextPoolMethod = GetAddDbContextPoolMethod();

            //iterate over all DbContextBase classes 
            foreach (var dbContextBaseConfig in dbContextBaseConfigs) {

                //get the relevant configuration
                var dbContextBaseType = dbContextBaseTypes
                    .Where(x => x.Name == dbContextBaseConfig.Key).FirstOrDefault();

                //get the connection string
                var cxnString = dbContextBaseConfig.Value;

                //get a reference to the MethodInfo for AddDbContextPool
                MethodInfo addDbContextPoolMethodGeneric =
                    addDbContextPoolMethod.MakeGenericMethod(dbContextBaseType);

                //build the options for the HttpClient object
                Action<DbContextOptionsBuilder> options = opt => {
                    opt.UseSqlServer(cxnString);
                };

                //invoke the dependency injection method
                addDbContextPoolMethodGeneric.Invoke(services,
                    new object[] { services, options, poolSize });
            }

        }

        /// <summary>
        /// Configures dependency injection (DI) for all classes
        /// implementing IRepo.
        /// The configuration uses the standard DI extension 
        /// method AddTransient to create new instances of
        /// repositories having a Transient scope.
        /// </summary>
        /// <param name="services">a reference to IServiceCollection</param>
        public static void AddRepos(this IServiceCollection services) {

            //get all classes implementing IRepo
            var repoTypes = GetIRepoTypes();

            //get a reference to the AddTransient method
            var addTransientMethod = GetAddTransientMethod();

            //iterate over all classes implementing IRepo
            foreach (var repoType in repoTypes) {

                //get a reference to the MethodInfo for AddTransient
                MethodInfo addTransientMethodGeneric =
                    addTransientMethod.MakeGenericMethod(repoType,repoType);

                //invoke the dependency injection method
                addTransientMethodGeneric.Invoke(services,
                    new object[] { services });
            }
        }


        /// <summary>
        /// Gets the MethodInfo for the AddSingleton extension method
        /// </summary>
        /// <returns>AddSingleton extension method</returns>
        private static MethodInfo GetAddSingletonMethod() {

            var addSingletonMethodToString = "Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton[TService](Microsoft.Extensions.DependencyInjection.IServiceCollection, TService)";

            var method = typeof(IServiceCollection)
                .GetExtensionMethods(typeof(IServiceCollection).Assembly)
                .Where(x => x.ToString() == addSingletonMethodToString)
                .FirstOrDefault();

            return method;
        }


        /// <summary>
        /// Gets the MethodInfo for the AddTransient extension method
        /// </summary>
        /// <returns>AddTransient extension method</returns>
        private static MethodInfo GetAddTransientMethod() {

            var addTransientMethodToString = "Microsoft.Extensions.DependencyInjection.IServiceCollection AddTransient[TService,TImplementation](Microsoft.Extensions.DependencyInjection.IServiceCollection)";

            var method = typeof(IServiceCollection)
                .GetExtensionMethods(typeof(IServiceCollection).Assembly)
                .Where(x => x.ToString() == addTransientMethodToString)
                .FirstOrDefault();

            return method;
        }


        /// <summary>
        /// Gets the MethodInfo for the AddHttpClient extension method
        /// </summary>
        /// <returns>AddHttpClient extension method</returns>
        private static MethodInfo GetAddHttpClientMethod() {
            var assembly = typeof(HttpClientFactoryOptions).Assembly;
            var method = typeof(IServiceCollection)
                .GetExtensionMethods(assembly)
                .Where(x => x.ToString() == "Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddHttpClient[TClient](Microsoft.Extensions.DependencyInjection.IServiceCollection, System.Action`1[System.Net.Http.HttpClient])")
                .FirstOrDefault();
            return method;
        }


        /// <summary>
        /// Gets the MethodInfo for the AddDbContextPool extension method
        /// </summary>
        /// <returns>AddDbContextPool extension method</returns>
        private static MethodInfo GetAddDbContextPoolMethod() {
            var assembly = typeof(EntityFrameworkServiceCollectionExtensions).Assembly;
            var method = typeof(IServiceCollection)
                .GetExtensionMethods(assembly)
                .Where(x => x.ToString() == "Microsoft.Extensions.DependencyInjection.IServiceCollection AddDbContextPool[TContext](Microsoft.Extensions.DependencyInjection.IServiceCollection, System.Action`1[Microsoft.EntityFrameworkCore.DbContextOptionsBuilder], Int32)")
                .FirstOrDefault();
            return method;
        }


        /// <summary>
        /// Get the collection of all classes implementing IProxy
        /// </summary>
        /// <returns>all IApiProxy classes</returns>
        private static IEnumerable<Type> GetIApiProxyTypes() {
            var proxyTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t =>
                    t.GetInterfaces().Contains(typeof(IApiProxy)));
            //alternatively ... define RegisteredProxy attribute and use that
            //.Where(t => t.GetCustomAttribute<RegisteredProxy>() != null);
            return proxyTypes;
        }

        /// <summary>
        /// Get the collection of all classes extending DbContextBase
        /// </summary>
        /// <returns>all DbContextBase classes</returns>
        private static IEnumerable<Type> GetDbContextBaseTypes() {
            var serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(DbContextBase)));
            //alternatively ... define RegisteredDbContextBase attribute and use that
            //.Where(t => t.GetCustomAttribute<RegisteredDbContextBase>() != null);
            return serviceTypes;
        }

        /// <summary>
        /// Get the collection of all classes implementing IRepo
        /// </summary>
        /// <returns>all IRepo classes</returns>
        private static IEnumerable<Type> GetIRepoTypes() {
            var repoTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t =>
                    (t.GetInterfaces().Contains(typeof(IRepo))
                        && t.Name != "QueryableRepo`2"
                        && t.Name != "WriteableRepo`2"
                        && t.Name != "ResettableRepo`2"));
                    //alternatively ... define RegisteredRepo attribute and use that
                    //t.GetCustomAttribute<RegisteredRepo>() != null);
            return repoTypes;
        }

    }

}


