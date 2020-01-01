using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;


namespace EDennis.AspNetCore.Base.Testing {
    public class TestRepoFactory<TRepo, TEntity, TContext>
            where TEntity : class, IHasSysUser, new()
            where TContext : ResettableDbContext<TContext>
            where TRepo : IRepo<TEntity, TContext>  {

        public const string DEFAULT_USER = "tester@example.org";
        public const string DEFAULT_PKREWRITER_CONFIG_KEY = IServiceConfigExtensions.DEFAULT_PK_REWRITER_PATH;
        public const string DEFAULT_DBCONTEXT_CONFIG_KEY = IServiceConfigExtensions.DEFAULT_DBCONTEXTS_PATH;

        private IConfiguration _configuration;
        private PkRewriterSettings _pkRewriterSettings;
        private IScopeProperties _scopeProperties;
        private ILogger _logger;
        private IScopedLogger _scopedLogger;
        private IInterceptor[] _interceptors;
        private DbContextSettings<TContext> _dbContextSettings;
        private DbConnection<TContext> _dbConnection;
        private TContext _dbContext;

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
        public virtual PkRewriterSettings PkRewriterSettings {
            get {
                if (_pkRewriterSettings == null) {
                    _pkRewriterSettings = new PkRewriterSettings();
                    Configuration.GetSection(DEFAULT_PKREWRITER_CONFIG_KEY).Bind(_pkRewriterSettings);
                }
                return _pkRewriterSettings;
            }
            set {
                _pkRewriterSettings = value;
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
        public virtual ILogger Logger {
            get {
                if (_logger == null)
                    _logger = NullLogger.Instance;
                return NullLogger.Instance;
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

        public virtual IInterceptor[] Interceptors { 
            get {
                if (_interceptors == null) {
                    var devName = PkRewriterSettings.DeveloperNameEnvironmentVariable;
                    var devPrefix = PkRewriterSettings.DeveloperPrefixes[devName];
                    var basePrefix = PkRewriterSettings.BasePrefix;

                    _interceptors = new IInterceptor[]
                        {
                        new PkRewriterInterceptor(
                            new PkRewriter(devPrefix,basePrefix))
                        };
                }
                return _interceptors;
            } 
            set {
                _interceptors = value;
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

        public virtual DbConnection<TContext> DbConnection {
            get {
                if (_dbConnection == null) {
                    _dbConnection = DbConnectionManager.GetDbConnection(DbContextSettings.Interceptor);
                }
                return _dbConnection;
            }
            set {
                _dbConnection = value;
            }
        }

        public virtual TContext DbContext {
            get {
                if (_dbContext == null)
                    _dbContext = DbConnectionManager.GetDbContext(
                        DbConnection, DbContextSettings, Interceptors);
                return _dbContext;
            }
            set {
                _dbContext = value;
            }
        }


        public virtual TRepo CreateRepo() => (TRepo) Activator.CreateInstance(typeof(TRepo),
                new object[] { DbContext, ScopeProperties, Logger, ScopedLogger });


    }

}


