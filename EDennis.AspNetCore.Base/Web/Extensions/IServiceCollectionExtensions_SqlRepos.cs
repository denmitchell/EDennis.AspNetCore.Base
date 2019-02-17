using Microsoft.Extensions.DependencyInjection;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_SqlRepos {

        public static IServiceCollection AddSqlRepos<TRepo1>(this IServiceCollection services)
            where TRepo1 : class {
            services.AddScoped<TRepo1, TRepo1>();
            return services;
        }

        public static IServiceCollection AddSqlRepos<TRepo1, TRepo2>(this IServiceCollection services)
            where TRepo1: class
            where TRepo2: class {
            services.AddSqlRepos<TRepo1>();
            services.AddSqlRepos<TRepo2>();
            return services;
        }

        public static IServiceCollection AddSqlRepos<TRepo1, TRepo2, TRepo3>(this IServiceCollection services)
            where TRepo1 : class
            where TRepo2 : class
            where TRepo3 : class {
            services.AddSqlRepos<TRepo1, TRepo2>();
            services.AddSqlRepos<TRepo3>();
            return services;
        }

        public static IServiceCollection AddSqlRepos<TRepo1, TRepo2, TRepo3, TRepo4>(this IServiceCollection services)
            where TRepo1 : class
            where TRepo2 : class
            where TRepo3 : class
            where TRepo4 : class {
            services.AddSqlRepos<TRepo1, TRepo2, TRepo3>();
            services.AddSqlRepos<TRepo4>();
            return services;
        }
        public static IServiceCollection AddSqlRepos<TRepo1, TRepo2, TRepo3, TRepo4, TRepo5>(this IServiceCollection services)
            where TRepo1 : class
            where TRepo2 : class
            where TRepo3 : class
            where TRepo4 : class
            where TRepo5 : class {
            services.AddSqlRepos<TRepo1, TRepo2, TRepo3, TRepo4>();
            services.AddSqlRepos<TRepo5>();
            return services;
        }

    }
}

