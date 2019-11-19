using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EDennis.AspNetCore.Base {
    public class DbContextConfig {
        private readonly IServiceConfig _serviceConfig;

        public DbContextConfig(IServiceConfig serviceConfig, string path) {
            _serviceConfig = serviceConfig;
            _serviceConfig.Goto(path);
      }

        public DbContextConfig AddRepo<TRepo>()
            where TRepo : class, IRepo {
            _serviceConfig.Services.TryAddScoped<IScopeProperties, ScopeProperties>();
            _serviceConfig.Services.TryAddScoped<ScopeProperties, ScopeProperties>();
            _serviceConfig.Services.AddScoped<TRepo, TRepo>();
            return this;
        }


        public DbContextConfig AddDbContext<TContext>(string path)
            where TContext : DbContext
            => _serviceConfig.AddDbContext<TContext>(path);

        public DbContextConfig AddDbContext<TContext>()
            where TContext : DbContext =>
            AddDbContext<TContext>(typeof(TContext).Name);

        public IServiceConfig Goto(string path) =>
            _serviceConfig.Goto(path);

    }
}
