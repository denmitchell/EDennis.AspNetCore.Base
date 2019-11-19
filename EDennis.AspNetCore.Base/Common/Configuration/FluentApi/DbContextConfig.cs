using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.AspNetCore.Base {
    public class DbContextConfig {
        private readonly ServiceConfig _appConfig;
        private readonly IServiceCollection _services;
        private readonly IConfigurationSection _section;

        public DbContextConfig(IServiceCollection services, ServiceConfig appConfig, IConfigurationSection section, string configKey) {
            _services = services;
            _appConfig = appConfig;
            _section = section.GetSection(configKey);
        }

        public DbContextConfig AddRepo<TRepo>()
            where TRepo : class, IRepo {
            _services.TryAddScoped<IScopeProperties, ScopeProperties>();
            _services.TryAddScoped<ScopeProperties, ScopeProperties>();
            _services.AddScoped<TRepo, TRepo>();
            return this;
        }


        public DbContextConfig AddDbContext<TContext>(string configKey)
            where TContext : DbContext
            => _appConfig.AddDbContext<TContext>(configKey);

        public DbContextConfig AddDbContext<TContext>()
            where TContext : DbContext =>
            AddDbContext<TContext>(typeof(TContext).Name);

        public ServiceConfig GetSection(string configKey)
            => _appConfig.Goto(configKey);


    }
}
