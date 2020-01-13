using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;


namespace EDennis.AspNetCore.Base.Testing {
    public class TestRepoFactory<TRepo, TEntity, TContext>
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext
            where TRepo : IRepo<TEntity, TContext> {

        public const string DEFAULT_USER = "tester@example.org";
        public const string DEFAULT_DBCONTEXT_CONFIG_KEY = IServiceConfigExtensions.DEFAULT_DBCONTEXTS_PATH;

        private IConfiguration _configuration;
        private IScopeProperties _scopeProperties;
        private ILogger<TRepo> _logger;
        private IScopedLogger _scopedLogger;
        private DbContextSettings<TContext> _dbContextSettings;
        private CachedConnection<TContext> _cachedConnection;
        private DbContextProvider<TContext> _dbContextProvider;
        private StoredProcedureDefs<TContext> _storedProcedureDefs;


        public TestRepoFactory() {
            DbContext = DbContextProvider<TContext>.GetInterceptorContext(DbContextSettings, CachedConnection);
            if (DbContext is ISqlServerDbContext<TContext>)
                (DbContext as ISqlServerDbContext<TContext>).StoredProcedureDefs = StoredProcedureDefs;
        }


        public virtual IConfiguration Configuration {
            get {
                if (_configuration == null) {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    _configuration = new ConfigurationBuilder()
                        .AddJsonFile($"appsettings.{env}.json")
                        .Build();
                }
                return _configuration;
            }
            set {
                _configuration = value;
            }
        }

        public virtual IScopeProperties ScopeProperties {
            get {
                if (_scopeProperties == null)
                    _scopeProperties = new ScopeProperties { User = DEFAULT_USER };
                return _scopeProperties;
            }
            set {
                _scopeProperties = value;
            }
        }
        public virtual ILogger<TRepo> Logger {
            get {
                if (_logger == null)
                    _logger = NullLogger<TRepo>.Instance;
                return _logger;
            }
            set {
                _logger = value;
            }
        }

        public virtual IScopedLogger ScopedLogger {
            get {
                if (_scopedLogger == null)
                    _scopedLogger = new NullScopedLogger();
                return _scopedLogger;
            }
            set {
                _scopedLogger = value;
            }
        }


        public virtual DbContextSettings<TContext> DbContextSettings {
            get {
                if (_dbContextSettings == null) {
                    _dbContextSettings = new DbContextSettings<TContext>();
                    var configKey = $"{DEFAULT_DBCONTEXT_CONFIG_KEY}:{typeof(TContext).Name}";
                    Configuration.GetSection(configKey).Bind(_dbContextSettings);
                }
                return _dbContextSettings;
            }
            set {
                _dbContextSettings = value;
            }
        }


        public virtual CachedConnection<TContext> CachedConnection {
            get {
                if (_cachedConnection == null) {
                    _cachedConnection = new CachedConnection<TContext>();
                }
                return _cachedConnection;
            }
            set {
                _cachedConnection = value;
            }
        }

        public virtual TContext DbContext { get; set; }



        public virtual StoredProcedureDefs<TContext> StoredProcedureDefs {
            get {
                if (_storedProcedureDefs == null)
                    _storedProcedureDefs = new StoredProcedureDefs<TContext>(DbContext);
                return _storedProcedureDefs;
            }
            set {
                _storedProcedureDefs = value;
            }
        }


        public virtual DbContextProvider<TContext> DbContextProvider {
            get {
                if (_dbContextProvider == null) {
                    _dbContextProvider = new DbContextProvider<TContext>(DbContext, StoredProcedureDefs);
                }
                return _dbContextProvider;
            }
            set {
                _dbContextProvider = value;
            }
        }


        public virtual TRepo CreateRepo() => (TRepo)Activator.CreateInstance(typeof(TRepo),
                new object[] { DbContextProvider, ScopeProperties, Logger, ScopedLogger });


        public virtual void ResetRepo() {
            DbContextProvider<TContext>.Reset(DbContextSettings, CachedConnection);
        }

    }

}


