using Castle.DynamicProxy;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_TraceInterceptor {

        }

        public static IServiceCollection AddScoped<TRepo, TContext>(this IServiceCollection services)
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



        public static IServiceCollection AddScoped<TClientInterface, TClientImplementation>(this IServiceCollection services)
            where TClientImplementation : ApiClient { 

            /*
        public SecureApiClient(HttpClient httpClient,
            IConfiguration config,
            ScopeProperties scopeProperties,
            ApiClient identityServerApiClient,
            SecureTokenCache secureTokenCache,
            IWebHostEnvironment hostingEnvironment)
            : base(httpClient, config, scopeProperties) {

            */

            return services.AddScoped(f => {
                var loggers = f.GetRequiredService<IEnumerable<ILogger<TClient>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);
                var context = f.GetRequiredService<TContext>();
                var repo = (TRepo)new ProxyGenerator()
                    .CreateClassProxy(typeof(TRepo),
                        new object[] { context, scopeProperties, activeLogger },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });



        }
    }
}
