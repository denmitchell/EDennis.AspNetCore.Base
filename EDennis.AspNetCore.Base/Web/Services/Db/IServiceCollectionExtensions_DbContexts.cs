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

        public static IServiceCollection AddDbContext<TContext>(this IServiceCollection services, 
            IConfiguration configuration, string configurationKey)
            where TContext : DbContext {

            var configSection = configuration.GetSection(configurationKey);
            services.Configure<DbContextSettings<TContext>>(configSection);

            var settings = new DbContextSettings<TContext>();
            configSection.Bind(settings);

            var options = settings.DatabaseProvider switch
            {
                DatabaseProvider.SqlServer => new Action<DbContextOptionsBuilder>(options=> { options.UseSqlServer(settings.ConnectionString); }),
                DatabaseProvider.Sqlite => new Action<DbContextOptionsBuilder>(options => { options.UseSqlite(settings.ConnectionString); }),
                DatabaseProvider.InMemory => new Action<DbContextOptionsBuilder>(options => { options.UseInMemoryDatabase(settings.ConnectionString ?? Guid.NewGuid().ToString()); }),
                _ => new Action<DbContextOptionsBuilder>(options => { })
                };

            services.AddDbContext<TContext>(options);
            services.AddScoped<DbContextOptionsProvider<TContext>>();

            services.TryAddSingleton<DbConnectionCache<TContext>>();

            return services;
        }


    }
}
