using Castle.DynamicProxy;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_Repos {


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

        private static IServiceCollection AddScoped<TRepoImplementation, TContext>(this IServiceCollection services)
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


        private static IServiceCollection AddScoped<TRepoInterface, TRepoImplementation, TEntity, TContext, THistoryContext>(this IServiceCollection services)
            where TRepoInterface : class
            where TEntity : class, IEFCoreTemporalModel, new()
            where TRepoImplementation : WriteableTemporalRepo<TEntity,TContext,THistoryContext>, TRepoInterface
            where TContext : DbContext
            where THistoryContext : DbContext {
            return services.AddScoped<TRepoInterface>(f => {
                var loggers = f.GetRequiredService<IEnumerable<ILogger<TRepoInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);
                var context = f.GetRequiredService<TContext>();
                var histContext = f.GetRequiredService<THistoryContext>();
                var repo = (TRepoImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TRepoImplementation),
                        new object[] { context, scopeProperties, activeLogger },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }

        private static IServiceCollection AddScoped<TRepoImplementation, TEntity, TContext, THistoryContext>(this IServiceCollection services)
            where TEntity : class, IEFCoreTemporalModel, new()
            where TRepoImplementation : WriteableTemporalRepo<TEntity, TContext, THistoryContext>
            where TContext : DbContext
            where THistoryContext : DbContext {
            return services.AddScoped(f => {
                var loggers = f.GetRequiredService<IEnumerable<ILogger<TRepoImplementation>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);
                var context = f.GetRequiredService<TContext>();
                var histContext = f.GetRequiredService<THistoryContext>();
                var repo = (TRepoImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TRepoImplementation),
                        new object[] { context, scopeProperties, activeLogger },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }

        #region Obsolete

        [Obsolete("Use AddRepo, instead")]
        public static IServiceCollection AddRepos<TRepo1, TContext>(this IServiceCollection services)
            where TRepo1 : class, IRepo
            where TContext : DbContext {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TRepo1, TContext>();
            return services;
        }

        [Obsolete("Use AddRepo, instead")]
        public static IServiceCollection AddRepos<TRepo1, TRepo2, TContext>(this IServiceCollection services)
            where TRepo1 : class, IRepo
            where TRepo2 : class, IRepo
            where TContext : DbContext {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TRepo1, TContext>();
            services.AddScoped<TRepo2, TContext>();
            return services;
        }

        [Obsolete("Use AddRepo, instead")]
        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3, TContext>(this IServiceCollection services)
            where TRepo1 : class, IRepo
            where TRepo2 : class, IRepo
            where TRepo3 : class, IRepo
            where TContext : DbContext {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TRepo1, TContext>();
            services.AddScoped<TRepo2, TContext>();
            services.AddScoped<TRepo3, TContext>();
            return services;
        }


        [Obsolete("Use AddRepo, instead")]
        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3, TRepo4, TContext>(this IServiceCollection services)
            where TRepo1 : class, IRepo
            where TRepo2 : class, IRepo
            where TRepo3 : class, IRepo
            where TRepo4 : class, IRepo
            where TContext : DbContext {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TRepo1, TContext>();
            services.AddScoped<TRepo2, TContext>();
            services.AddScoped<TRepo3, TContext>();
            services.AddScoped<TRepo3, TContext>();
            services.AddScoped<TRepo4, TContext>();
            return services;
        }

        [Obsolete("Use AddRepo, instead")]
        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3, TRepo4, TRepo5, TContext>(this IServiceCollection services)
            where TRepo1 : class, IRepo
            where TRepo2 : class, IRepo
            where TRepo3 : class, IRepo
            where TRepo4 : class, IRepo
            where TRepo5 : class, IRepo
            where TContext : DbContext {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TRepo1, TContext>();
            services.AddScoped<TRepo2, TContext>();
            services.AddScoped<TRepo3, TContext>();
            services.AddScoped<TRepo3, TContext>();
            services.AddScoped<TRepo4, TContext>();
            services.AddScoped<TRepo5, TContext>();
            return services;
        }

        #endregion

    }
}

