using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace EDennis.AspNetCore.Base.Web {

    public interface IDbContextSettingsBuilder<TContext>
        where TContext : DbContext { 
        DbContextSettings<TContext> DbContextSettings { get; set; }
        IServiceCollection ServiceCollection { get; set; } 
        IConfiguration Configuration { get; set; }
        string ConfigurationKey { get; set; }
    }

    public class DbContextSettingsBuilder<TContext> : IDbContextSettingsBuilder<TContext>
        where TContext : DbContext {
        public DbContextSettings<TContext> DbContextSettings { get; set; }
        public IServiceCollection ServiceCollection { get; set; }
        public IConfiguration Configuration { get; set; }
        public string ConfigurationKey { get; set; }
    }



    public static class IDbContextSettingsBuilder_Extensions {

        public static IDbContextSettingsBuilder<TContext> AddInterceptor<TContext>(this IDbContextSettingsBuilder<TContext> builder)
            where TContext: DbContext
            {
            builder.ServiceCollection.Configure<DbContextInterceptorSettings<TContext>>(options => {
                options.DatabaseProvider = builder.DbContextSettings.DatabaseProvider;
                options.ConnectionString = builder.DbContextSettings.ConnectionString;
                options.InstanceNameSource = builder.DbContextSettings.Interceptor.InstanceNameSource;
                options.IsInMemory = builder.DbContextSettings.Interceptor.IsInMemory;
                options.IsolationLevel = builder.DbContextSettings.Interceptor.IsolationLevel;
            });

            return builder;
        }

        public static IServiceCollection AddRepo<TRepo>(this IServiceCollection services, bool traceable = true)
            where TRepo : class, IHasILogger, IRepo{
            services.TryAddScoped<IScopeProperties, ScopeProperties>();
            if (traceable)
                services.AddScopedTraceable<TRepo>();
            else
                services.AddScoped<TRepo, TRepo>();
            return services;
        }


    }
}
