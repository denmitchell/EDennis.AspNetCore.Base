using Microsoft.Extensions.DependencyInjection;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_Repos {

        public static IServiceCollection AddRepos<TRepo1>(this IServiceCollection services)
            where TRepo1 : class {
            services.AddScoped<TRepo1, TRepo1>();
            return services;
        }

        public static IServiceCollection AddRepos<TRepo1, TRepo2>(this IServiceCollection services)
            where TRepo1: class
            where TRepo2: class {
            services.AddRepos<TRepo1>();
            services.AddRepos<TRepo2>();
            return services;
        }

        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3>(this IServiceCollection services)
            where TRepo1 : class
            where TRepo2 : class
            where TRepo3 : class {
            services.AddRepos<TRepo1, TRepo2>();
            services.AddRepos<TRepo3>();
            return services;
        }

        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3, TRepo4>(this IServiceCollection services)
            where TRepo1 : class
            where TRepo2 : class
            where TRepo3 : class
            where TRepo4 : class {
            services.AddRepos<TRepo1, TRepo2, TRepo3>();
            services.AddRepos<TRepo4>();
            return services;
        }
        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3, TRepo4, TRepo5>(this IServiceCollection services)
            where TRepo1 : class
            where TRepo2 : class
            where TRepo3 : class
            where TRepo4 : class
            where TRepo5 : class {
            services.AddRepos<TRepo1, TRepo2, TRepo3, TRepo4>();
            services.AddRepos<TRepo5>();
            return services;
        }

    }
}

