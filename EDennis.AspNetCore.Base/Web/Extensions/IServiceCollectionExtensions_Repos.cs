using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_Repos {

        public static IServiceCollection AddRepos<TRepo>(this IServiceCollection services)
            where TRepo : class, IRepo {
            services.TryAddScoped<ScopeProperties>();
            services.AddScoped<TRepo, TRepo>();
            return services;
        }

        public static IServiceCollection AddRepos<TRepo1,TRepo2>(this IServiceCollection services)
            where TRepo1 : class, IRepo
            where TRepo2 : class, IRepo {
            services.AddRepos<TRepo1>();
            services.AddScoped<TRepo2, TRepo2>();
            return services;
        }

        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3>(this IServiceCollection services)
            where TRepo1 : class, IRepo
            where TRepo2 : class, IRepo
            where TRepo3 : class, IRepo {
            services.AddRepos<TRepo1, TRepo2>();
            services.AddScoped<TRepo3, TRepo3>();
            return services;
        }


        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3, TRepo4>(this IServiceCollection services)
            where TRepo1 : class, IRepo
            where TRepo2 : class, IRepo
            where TRepo3 : class, IRepo
            where TRepo4 : class, IRepo {
            services.AddRepos<TRepo1, TRepo2, TRepo3>();
            services.AddScoped<TRepo4, TRepo4>();
            return services;
        }

        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3, TRepo4, TRepo5>(this IServiceCollection services)
            where TRepo1 : class, IRepo
            where TRepo2 : class, IRepo
            where TRepo3 : class, IRepo
            where TRepo4 : class, IRepo
            where TRepo5 : class, IRepo {
            services.AddRepos<TRepo1, TRepo2, TRepo3, TRepo4>();
            services.AddScoped<TRepo5, TRepo5>();
            return services;
        }

    }
}

