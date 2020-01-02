using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.AspNetCore.Base.Testing {
    public class TestTemporalRepoFactory<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext> 
        : TestRepoFactory<TTemporalRepo, TEntity, TContext >
            where TEntity : class, IHasSysUser, IEFCoreTemporalModel, new()
            where THistoryEntity : TEntity
            where TContext : ResettableDbContext<TContext>
            where THistoryContext : ResettableDbContext<THistoryContext>
            where TTemporalRepo : ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext>{

        private DbContextSettings<THistoryContext> _historyDbContextSettings;
        private DbConnection<THistoryContext> _historyDbConnection;
        private THistoryContext _historyDbContext;

        public virtual DbContextSettings<THistoryContext> HistoryDbContextSettings {
            get {
                if (_historyDbContextSettings == null) {
                    _historyDbContextSettings = new DbContextSettings<THistoryContext>();
                    var configKey = $"{DEFAULT_DBCONTEXT_CONFIG_KEY}:{typeof(THistoryContext).Name}";
                    Configuration.GetSection(configKey).Bind(_historyDbContextSettings);
                }
                return _historyDbContextSettings;
            }
            set {
                _historyDbContextSettings = value;
            }
        }

        public virtual DbConnection<THistoryContext> HistoryDbConnection {
            get {
                if (_historyDbConnection == null) {
                    _historyDbConnection = DbConnectionManager.GetDbConnection(HistoryDbContextSettings.Interceptor);
                }
                return _historyDbConnection;
            }
            set {
                _historyDbConnection = value;
            }
        }

        public virtual THistoryContext HistoryDbContext {
            get {
                if (_historyDbContext == null)
                    _historyDbContext = DbConnectionManager.GetDbContext(
                        HistoryDbConnection, HistoryDbContextSettings, Interceptors);
                return _historyDbContext;
            }
            set {
                _historyDbContext = value;
            }
        }


        public new virtual TTemporalRepo CreateRepo() => (TTemporalRepo)Activator.CreateInstance(typeof(TTemporalRepo),
                new object[] { DbContext, HistoryDbContext, ScopeProperties, Logger, ScopedLogger });



    }
}
