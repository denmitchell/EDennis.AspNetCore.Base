using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_DbContexts {

        public static IServiceCollection AddDbContexts<TContext>(this IServiceCollection services, 
            IConfiguration configuration, string configurationKey)
            where TContext : DbContext {

            var configSection = configuration.GetSection(configurationKey);
            services.Configure<DbContextSettings<TContext>>(configSection);

            var settings = new DbContextSettings<TContext>();
            configSection.Bind(settings);

            services.AddDbContext<TContext>(builder => { DbConnectionManager.ConfigureDbContextOptionsBuilder(builder, settings); });
            services.AddScoped<DbContextOptionsProvider<TContext>>();

            services.TryAddSingleton<DbConnectionCache<TContext>>();

            return services;
        }


    }
}
