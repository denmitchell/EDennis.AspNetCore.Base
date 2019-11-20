using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Data;
using System.Data.SqlClient;


namespace EDennis.AspNetCore.Base.Testing {
    public abstract class TemporalRepoFixture<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext> : IDisposable
        where TTemporalRepo : ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext>
        where TEntity : class, IEFCoreTemporalModel, new()
        where THistoryEntity : TEntity
        where TContext : DbContext
        where THistoryContext: DbContext {

        public const string DEFAULT_USER = "tester@example.org";

        public virtual IScopeProperties ScopeProperties { get; } =
            new ScopeProperties { User = DEFAULT_USER };

        public virtual IConfiguration Configuration { get; set; } =
            new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
            .AddJsonFile("appsettings.Shared.json", true, true)
            .Build();

        public virtual string DbContextConfigurationKey { get; } = "DbContexts:" + typeof(TContext).Name;
        public virtual string HistoryDbContextConfigurationKey { get; } = "DbContexts:" + typeof(THistoryContext).Name;

        public virtual Action<IDbConnection, DbContextSettings<TContext>> ResetAction {
            get {
                if (DbConnection.IDbConnection is SqlConnection)
                    return DbConnectionManager.Reset;
                else
                    return null;
            }
        }

        public virtual Action<IDbConnection, DbContextSettings<THistoryContext>> HistoryResetAction {
            get {
                if (DbConnection.IDbConnection is SqlConnection)
                    return DbConnectionManager.Reset;
                else
                    return null;
            }
        }


        public DbConnection<TContext> DbConnection { get; private set; }
        public DbConnection<THistoryContext> HistoryDbConnection { get; private set; }

        public DbContextSettings<TContext> DbContextSettings { get; private set; }
        public DbContextSettings<THistoryContext> HistoryDbContextSettings { get; private set; }

        public TTemporalRepo Repo;

        public TemporalRepoFixture(){
            DbContextSettings = GetSettings();
            HistoryDbContextSettings = GetHistorySettings();

            DbConnection = DbConnectionManager.GetDbConnection(DbContextSettings.Interceptor);
            HistoryDbConnection = DbConnectionManager.GetDbConnection(HistoryDbContextSettings.Interceptor);

            var context = DbConnectionManager.GetDbContext(DbConnection, DbContextSettings);
            var historyContext = DbConnectionManager.GetDbContext(HistoryDbConnection, HistoryDbContextSettings);

            Repo = (TTemporalRepo)Activator.CreateInstance(typeof(TTemporalRepo),
                new object[] { context, historyContext, ScopeProperties, NullLogger.Instance, new NullScopedLogger() });

        }

        public void Reset() {
            ResetAction?.Invoke(DbConnection.IDbConnection, DbContextSettings);
            HistoryResetAction?.Invoke(HistoryDbConnection.IDbConnection, HistoryDbContextSettings);
            DbConnection = DbConnectionManager.GetDbConnection(DbContextSettings.Interceptor);
            HistoryDbConnection = DbConnectionManager.GetDbConnection(HistoryDbContextSettings.Interceptor);
            Repo.Context = DbConnectionManager.GetDbContext(DbConnection, DbContextSettings);
            Repo.HistoryContext = DbConnectionManager.GetDbContext(HistoryDbConnection, HistoryDbContextSettings);
        }


        private DbContextSettings<TContext> GetSettings() {
            var settings = new DbContextSettings<TContext>();
            Configuration.GetSection(DbContextConfigurationKey).Bind(settings);
            return settings;            
        }
        private DbContextSettings<THistoryContext> GetHistorySettings() {
            var settings = new DbContextSettings<THistoryContext>();
            Configuration.GetSection(DbContextConfigurationKey).Bind(settings);
            return settings;
        }


        public void Dispose() {
            Reset();
        }



    }
}
