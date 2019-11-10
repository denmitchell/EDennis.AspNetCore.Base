using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_Repos {

        public static IServiceCollection AddRepo<TRepo>(this IServiceCollection services, bool traceable = true)
            where TRepo : class, IHasILogger, IRepo22 {
            services.TryAddScoped<ScopeProperties22>();
            if (traceable)
                services.AddScopedTraceable<TRepo>();
            else
                services.AddScoped<TRepo, TRepo>();
            return services;
        }

        public static IServiceCollection AddRepos<TRepo>(this IServiceCollection services, bool traceable = true)
            where TRepo : class, IHasILogger, IRepo22
            => services.AddRepo<TRepo>(traceable);


        public static IServiceCollection AddRepos<TRepo1, TRepo2>(this IServiceCollection services, bool traceable = true)
            where TRepo1 : class, IHasILogger, IRepo22
            where TRepo2 : class, IHasILogger, IRepo22 {
            services.AddRepos<TRepo1>();
            if (traceable)
                services.AddScopedTraceable<TRepo2>();
            else
                services.AddScoped<TRepo2, TRepo2>();
            return services;
        }

        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3>(this IServiceCollection services, bool traceable = true)
            where TRepo1 : class, IHasILogger, IRepo22
            where TRepo2 : class, IHasILogger, IRepo22
            where TRepo3 : class, IHasILogger, IRepo22 {
            services.AddRepos<TRepo1, TRepo2>();
            if (traceable)
                services.AddScopedTraceable<TRepo3>();
            else
                services.AddScoped<TRepo3, TRepo3>();
            return services;
        }


        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3, TRepo4>(this IServiceCollection services, bool traceable = true)
            where TRepo1 : class, IHasILogger, IRepo22
            where TRepo2 : class, IHasILogger, IRepo22
            where TRepo3 : class, IHasILogger, IRepo22
            where TRepo4 : class, IHasILogger, IRepo22 {
            services.AddRepos<TRepo1, TRepo2, TRepo3>();
            if (traceable)
                services.AddScopedTraceable<TRepo4>();
            else
                services.AddScoped<TRepo4, TRepo4>();
            return services;
        }

        public static IServiceCollection AddRepos<TRepo1, TRepo2, TRepo3, TRepo4, TRepo5>(this IServiceCollection services, bool traceable = true)
            where TRepo1 : class, IHasILogger, IRepo22
            where TRepo2 : class, IHasILogger, IRepo22
            where TRepo3 : class, IHasILogger, IRepo22
            where TRepo4 : class, IHasILogger, IRepo22
            where TRepo5 : class, IHasILogger, IRepo22 {
            services.AddRepos<TRepo1, TRepo2, TRepo3, TRepo4>();
            if (traceable)
                services.AddScopedTraceable<TRepo5>();
            else
                services.AddScoped<TRepo5, TRepo5>();
            return services;
        }

    }
}

