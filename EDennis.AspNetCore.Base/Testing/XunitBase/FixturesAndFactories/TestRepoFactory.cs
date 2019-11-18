using Castle.Core.Logging;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.AspNetCore.Base.Testing {
    public class TestRepoFactory {

        public const string DEFAULT_USER = "tester@example.org";


        public static TRepo CreateRepo<TRepo, TEntity, TContext>(IConfiguration config)
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext
            where TRepo : IRepo<TEntity, TContext>
            => CreateRepo<TRepo, TEntity, TContext>(config, typeof(TContext).Name,
                new ScopeProperties { User = DEFAULT_USER }
                );


        public static TRepo CreateRepo<TRepo, TEntity, TContext>(IConfiguration config,
            string dbContextConfigKey)
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext
            where TRepo : IRepo<TEntity, TContext>
            => CreateRepo<TRepo, TEntity, TContext>(config, dbContextConfigKey,
                new ScopeProperties { User = DEFAULT_USER }
                );


        public static TRepo CreateRepo<TRepo, TEntity, TContext>(IConfiguration config,
        string dbContextConfigKey, IScopeProperties scopeProperties)
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : IRepo<TEntity, TContext> {

            var settings = new DbContextSettings<TContext>();
            config.GetSection(dbContextConfigKey).Bind(settings);

            var context = DbConnectionManager.GetTestDbContext(settings.Interceptor);

            var repo = (TRepo)Activator.CreateInstance(typeof(TRepo),
                new object[] { context, scopeProperties, NullLogger.Instance, new NullScopedLogger() });

            return repo;
        }

        public static TTemporalRepo CreateTemporalRepo<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>(IConfiguration config,
            string dbContextConfigKey, string historyDbContextConfigKey)
            where TEntity : class, IEFCoreTemporalModel, new()
            where THistoryEntity : TEntity
            where TContext : DbContext
            where THistoryContext : DbContext
            where TTemporalRepo : ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext>
            => CreateTemporalRepo<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>(
                config, dbContextConfigKey, historyDbContextConfigKey,
                new ScopeProperties { User = DEFAULT_USER }
                );


        public static TTemporalRepo CreateTemporalRepo<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>(
            IConfiguration config)
            where TEntity : class, IEFCoreTemporalModel, new()
            where THistoryEntity : TEntity
            where TContext : DbContext
            where THistoryContext : DbContext
            where TTemporalRepo : ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext>
            => CreateTemporalRepo<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>(
                config, typeof(TContext).Name, typeof(THistoryContext).Name,
                new ScopeProperties { User = DEFAULT_USER }
                );


        public static TTemporalRepo CreateTemporalRepo<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>(IConfiguration config,
            string dbContextConfigKey, string historyDbContextConfigKey,
            IScopeProperties scopeProperties)
            where TEntity : class, IEFCoreTemporalModel, new()
            where THistoryEntity : TEntity
            where TContext : DbContext
            where THistoryContext : DbContext
            where TTemporalRepo : ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext> {

            var settings = new DbContextSettings<TContext>();
            config.GetSection(dbContextConfigKey).Bind(settings);

            var historySettings = new DbContextSettings<THistoryContext>();
            config.GetSection(historyDbContextConfigKey).Bind(settings);

            var context = DbConnectionManager.GetTestDbContext(settings.Interceptor);
            var historyContext = DbConnectionManager.GetTestDbContext(historySettings.Interceptor);

            var repo = (TTemporalRepo)Activator.CreateInstance(typeof(TTemporalRepo),
                new object[] { context, historyContext, scopeProperties, NullLogger.Instance, new NullScopedLogger() });

            return repo;
        }


    }

}


