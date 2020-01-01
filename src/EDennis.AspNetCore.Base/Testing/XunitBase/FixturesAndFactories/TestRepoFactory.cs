using Castle.Core.Logging;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;


namespace EDennis.AspNetCore.Base.Testing {
    public class TestRepoFactory {

        public const string DEFAULT_USER = "tester@example.org";


        public static TRepo CreateRepo<TRepo, TEntity, TContext>()
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext
            where TRepo : IRepo<TEntity, TContext> 
            => CreateRepo<TRepo, TEntity, TContext>(new ConfigurationFixture().Configuration, 
                typeof(TContext).Name,
                new ScopeProperties { User = DEFAULT_USER }
                );
        

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
        string dbContextConfigKey, IScopeProperties scopeProperties, 
        string pkRewriterConfigKey = IServiceConfigExtensions.DEFAULT_PK_REWRITER_PATH)
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : IRepo<TEntity, TContext> {

            var settings = GetDbContextSettings<TContext>(config, dbContextConfigKey);
            var dbConnection = DbConnectionManager.GetDbConnection(settings.Interceptor);

            var pkRewriterSettings = GetPkRewriterSettings(config, pkRewriterConfigKey);
            var devName = config[pkRewriterSettings.DeveloperNameEnvironmentVariable];
            var devPrefix = pkRewriterSettings.DeveloperPrefixes[devName];
            var basePrefix = pkRewriterSettings.BasePrefix;

            var interceptors = new IInterceptor[]
                { new PkRewriterInterceptor(
                    new PkRewriter(devPrefix,basePrefix))
                };
            var context = DbConnectionManager.GetDbContext(dbConnection, settings, interceptors);

            return (TRepo)Activator.CreateInstance(typeof(TRepo),
                new object[] { context, scopeProperties, NullLogger.Instance, new NullScopedLogger() });
        }



        public static TTemporalRepo CreateTemporalRepo<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>()
            where TEntity : class, IEFCoreTemporalModel, new()
            where THistoryEntity : TEntity
            where TContext : DbContext
            where THistoryContext : DbContext
            where TTemporalRepo : ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext>
            => CreateTemporalRepo<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>(
                new ConfigurationFixture().Configuration, typeof(TContext).Name, typeof(THistoryContext).Name,
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


        public static TTemporalRepo CreateTemporalRepo<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>(IConfiguration config,
            string dbContextConfigKey, string historyDbContextConfigKey,
            IScopeProperties scopeProperties,
            string pkRewriterConfigKey = IServiceConfigExtensions.DEFAULT_PK_REWRITER_PATH)
            where TEntity : class, IEFCoreTemporalModel, new()
            where THistoryEntity : TEntity
            where TContext : DbContext
            where THistoryContext : DbContext
            where TTemporalRepo : ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext> {


            var pkRewriterSettings = GetPkRewriterSettings(config, pkRewriterConfigKey);
            var devName = config[pkRewriterSettings.DeveloperNameEnvironmentVariable];
            var devPrefix = pkRewriterSettings.DeveloperPrefixes[devName];
            var basePrefix = pkRewriterSettings.BasePrefix;

            var interceptors = new IInterceptor[]
                { new PkRewriterInterceptor(
                    new PkRewriter(devPrefix,basePrefix))
                };

            var settings = GetDbContextSettings<TContext>(config, dbContextConfigKey);
            var dbConnection = DbConnectionManager.GetDbConnection(settings.Interceptor);
            var context = DbConnectionManager.GetDbContext(dbConnection, settings,interceptors);

            var historySettings = GetDbContextSettings<THistoryContext>(config, historyDbContextConfigKey);
            var historyDbConnection = DbConnectionManager.GetDbConnection(historySettings.Interceptor);
            var historyContext = DbConnectionManager.GetDbContext(historyDbConnection, historySettings, interceptors);


            var repo = (TTemporalRepo)Activator.CreateInstance(typeof(TTemporalRepo),
                new object[] { context, historyContext, scopeProperties, NullLogger.Instance, new NullScopedLogger() });

            return repo;
        }





        private static DbContextSettings<TContext> GetDbContextSettings<TContext>(IConfiguration config, string configKey)
            where TContext : DbContext {
            var settings = new DbContextSettings<TContext>();
            config.GetSection(configKey).Bind(settings);
            return settings;
        }

        private static PkRewriterSettings GetPkRewriterSettings(IConfiguration config, string configKey){
            var settings = new PkRewriterSettings();
            config.GetSection(configKey).Bind(settings);
            return settings;
        }



    }

}


