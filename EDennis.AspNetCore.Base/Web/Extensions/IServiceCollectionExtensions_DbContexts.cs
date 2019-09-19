using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_DbContexts {

        public static IServiceCollection AddDbContexts<TContext>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext : DbContext {

            var cxnStrings = new Dictionary<string, string>();
            config.GetSection("ConnectionStrings").Bind(cxnStrings);

            if (!cxnStrings.ContainsKey(typeof(TContext).Name))
                throw new ApplicationException($"Cannot find required entry 'ConnectionStrings:{typeof(TContext).Name}' in the Configuration (e.g., appsettings.{env}.json)");

            services.AddDbContext<TContext>(
                    options => {
                        options.UseSqlServer(cxnStrings[typeof(TContext).Name]);
                    }
                );

            if (env.EnvironmentName == EnvironmentName.Development
                || env.EnvironmentName == "LocalDevelopment"
                || env.EnvironmentName == "Local")
                services.AddSingleton<TestDbContextOptionsCache<TContext>>();

            return services;
        }


        public static IServiceCollection AddDbContexts<TContext1, TContext2>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext1 : DbContext 
            where TContext2 : DbContext {

            services.AddDbContexts<TContext1>(config, env);
            services.AddDbContexts<TContext2>(config, env);

            return services;
        }


        public static IServiceCollection AddDbContexts<TContext1, TContext2, TContext3>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext {

            services.AddDbContexts<TContext1,TContext2>(config, env);
            services.AddDbContexts<TContext3>(config, env);

            return services;
        }


        public static IServiceCollection AddDbContexts<TContext1, TContext2, TContext3, TContext4>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext {

            services.AddDbContexts<TContext1, TContext2, TContext3>(config, env);
            services.AddDbContexts<TContext4>(config, env);

            return services;
        }



        public static IServiceCollection AddDbContexts<TContext1, TContext2, TContext3, TContext4, TContext5>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext
            where TContext5 : DbContext {

            services.AddDbContexts<TContext1, TContext2, TContext3, TContext4>(config, env);
            services.AddDbContexts<TContext5>(config, env);

            return services;
        }

    }
}
