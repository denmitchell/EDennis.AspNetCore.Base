using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Data;
using System.Data.SqlClient;


namespace EDennis.AspNetCore.Base.Testing {
    public abstract class RepoFixture<TRepo, TEntity, TContext> : IDisposable
        where TRepo : IRepo<TEntity, TContext>
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext {

        public const string DEFAULT_USER = "tester@example.org";

        public virtual IScopeProperties ScopeProperties { get; } = 
            new ScopeProperties { User = DEFAULT_USER };

        public virtual Action<IDbConnection,DbContextSettings<TContext>> ResetAction 
            {
            get {
                if (DbConnection.IDbConnection is SqlConnection)
                    return DbConnectionManager.Reset;
                else
                    return null;
            }
        }



        public virtual IConfiguration Configuration { get; set; } =
            new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
            .AddJsonFile("appsettings.Shared.json", true, true)
            .Build();


        public virtual string DbContextConfigurationKey { get; } = "DbContexts:" + typeof(TContext).Name;


        public DbConnection<TContext> DbConnection { get; private set; }
        public DbContextSettings<TContext> DbContextSettings { get; private set; }
        public TRepo Repo;

        public RepoFixture(){
            DbContextSettings = GetSettings();            
            DbConnection = DbConnectionManager.GetDbConnection(DbContextSettings.Interceptor);
            var context = DbConnectionManager.GetDbContext(DbConnection, DbContextSettings);

            Repo = (TRepo)Activator.CreateInstance(typeof(TRepo),
                new object[] { context, ScopeProperties, NullLogger.Instance, new NullScopedLogger() });

        }

        public void Reset() {
            ResetAction?.Invoke(DbConnection.IDbConnection, DbContextSettings);
            DbConnection = DbConnectionManager.GetDbConnection(DbContextSettings.Interceptor);
            Repo.Context = DbConnectionManager.GetDbContext(DbConnection, DbContextSettings);
        }



        private DbContextSettings<TContext> GetSettings() {
            var settings = new DbContextSettings<TContext>();
            Configuration.GetSection(DbContextConfigurationKey).Bind(settings);
            return settings;            
        }


        public void Dispose() {
            Reset();
        }



    }
}
