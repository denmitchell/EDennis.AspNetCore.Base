using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.AspNetCore.Base.Testing {
    public class TestTemporalRepoFactory<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext> 
        : TestRepoFactory<TTemporalRepo, TEntity, TContext >
            where TEntity : class, IHasSysUser, IEFCoreTemporalModel, new()
            where THistoryEntity : TEntity
            where TContext : DbContext
            where THistoryContext : DbContext
            where TTemporalRepo : ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext>{

        private DbContextSettings<THistoryContext> _historyDbContextSettings;
        private StoredProcedureDefs<THistoryContext> _historyStoredProcedureDefs;
        private CachedConnection<THistoryContext> _historyCachedConnection;
        private DbContextProvider<THistoryContext> _dbContextProvider;


        public TestTemporalRepoFactory() :base() {
            HistoryDbContext = DbContextProvider<THistoryContext>.GetInterceptorContext(HistoryDbContextSettings, HistoryCachedConnection);
            if (HistoryDbContext is ISqlServerDbContext<THistoryContext>)
                (HistoryDbContext as ISqlServerDbContext<THistoryContext>).StoredProcedureDefs = HistoryStoredProcedureDefs;
        }


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


        public virtual CachedConnection<THistoryContext> HistoryCachedConnection {
            get {
                if (_historyCachedConnection == null) {
                    _historyCachedConnection = new CachedConnection<THistoryContext>();
                }
                return _historyCachedConnection;
            }
            set {
                _historyCachedConnection = value;
            }
        }

        public virtual THistoryContext HistoryDbContext { get; set; }


        public virtual StoredProcedureDefs<THistoryContext> HistoryStoredProcedureDefs {
            get {
                if (_historyStoredProcedureDefs == null)
                    _historyStoredProcedureDefs = new StoredProcedureDefs<THistoryContext>(HistoryDbContext);
                return _historyStoredProcedureDefs;
            }
            set {
                _historyStoredProcedureDefs = value;
            }
        }

        public virtual DbContextProvider<THistoryContext> HistoryDbContextProvider {
            get {
                if (_dbContextProvider == null) {
                    _dbContextProvider = new DbContextProvider<THistoryContext>(HistoryDbContext, HistoryStoredProcedureDefs);
                }
                return _dbContextProvider;
            }
            set {
                _dbContextProvider = value;
            }
        }



        public new virtual TTemporalRepo CreateRepo() => (TTemporalRepo)Activator.CreateInstance(typeof(TTemporalRepo),
                new object[] { DbContextProvider, HistoryDbContextProvider, ScopeProperties, Logger, ScopedLogger });


        public new virtual void ResetRepo() {
            DbContextProvider<TContext>.Reset(DbContextSettings, CachedConnection);
            DbContextProvider<THistoryContext>.Reset(HistoryDbContextSettings, HistoryCachedConnection);
        }

    }
}
