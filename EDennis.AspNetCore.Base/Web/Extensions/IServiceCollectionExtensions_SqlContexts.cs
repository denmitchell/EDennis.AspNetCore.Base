using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_SqlContexts {

        public static IServiceCollection AddSqlContexts<TContext>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext : DbContext {

            var cxnStrings = new Dictionary<string, string>();
            config.GetSection("ConnectionStrings").Bind(cxnStrings);

            services.AddDbContext<TContext>(
                    options => {
                        options.UseSqlServer(cxnStrings[typeof(TContext).Name]);
                    }
                );

            if (env.EnvironmentName == EnvironmentName.Development)
                services.AddSingleton<TestDbContextCache<TContext>>();

            return services;
        }


        public static IServiceCollection AddSqlContexts<TContext1, TContext2>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext1 : DbContext 
            where TContext2 : DbContext {

            services.AddSqlContexts<TContext1>(config, env);
            services.AddSqlContexts<TContext2>(config, env);

            return services;
        }


        public static IServiceCollection AddSqlContexts<TContext1, TContext2, TContext3>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext {

            services.AddSqlContexts<TContext1,TContext2>(config, env);
            services.AddSqlContexts<TContext3>(config, env);

            return services;
        }


        public static IServiceCollection AddSqlContexts<TContext1, TContext2, TContext3, TContext4>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext {

            services.AddSqlContexts<TContext1, TContext2, TContext3>(config, env);
            services.AddSqlContexts<TContext4>(config, env);

            return services;
        }



        public static IServiceCollection AddSqlContexts<TContext1, TContext2, TContext3, TContext4, TContext5>(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext
            where TContext5 : DbContext {

            services.AddSqlContexts<TContext1, TContext2, TContext3, TContext4>(config, env);
            services.AddSqlContexts<TContext5>(config, env);

            return services;
        }

    }
}
