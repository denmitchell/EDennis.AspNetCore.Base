using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace EDennis.AspNetCore.Base.Testing {
    public class DbConnectionManager<TContext> where TContext : DbContext {




        public DbConnection<TContext> GetDbConnection(EFContext efContextSettings, string instanceName) {
            UseProvider(efContextSettings, instanceName, out DbContextOptions<TContext> options, out IDbConnection connection, out IDbTransaction transaction);
            return
                new DbConnection<TContext> {
                    DbContextOptions = options,
                    IDbConnection = connection,
                    IDbTransaction = transaction
                };
        }


        public virtual void UseProvider(EFContext efContextSettings, string instanceName, 
            out DbContextOptions<TContext> options,
            out IDbConnection connection, out IDbTransaction transaction) {
            switch (efContextSettings.ProviderName.ToLower()) {
                case "sqlserver":
                    UseSqlServer(efContextSettings, out options, out connection, out transaction);
                    return;
                case "sqlite":
                    UseSqlite(efContextSettings, out options, out connection, out transaction);
                    return;
                case "inmemory":
                    UseInMemory(instanceName, out options, out connection, out transaction);
                    return;
            }
            options = null;
            connection = null;
            transaction = null;
            return;
        }

        private void UseInMemory(string instanceName, out DbContextOptions<TContext> options, out IDbConnection connection, out IDbTransaction transaction) {
            connection = null;
            transaction = null;
            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseInMemoryDatabase($"{typeof(TContext).Name}-{instanceName}");
            options = builder.Options;
        }

        public void UseSqlServer(EFContext efContextSettings, out DbContextOptions<TContext> options, out IDbConnection connection, out IDbTransaction transaction) {
            connection = new SqlConnection(efContextSettings.ConnectionString);
            connection.Open();

            if (efContextSettings.TransactionType == TransactionType.Rollback) {
                transaction = connection.BeginTransaction(efContextSettings.IsolationLevel);
            } else {
                transaction = null;
            }
            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseSqlServer(connection as SqlConnection);
            options = builder.Options;
        }

        public void UseSqlite(EFContext efContextSettings, out DbContextOptions<TContext> options, out IDbConnection connection, out IDbTransaction transaction) {

            var connectionString = FixSqliteConnectionString(efContextSettings.ConnectionString, efContextSettings.IsolationLevel);
            connection = new SqliteConnection(connectionString);
            connection.Open();

            if (efContextSettings.TransactionType == TransactionType.Rollback) {
                transaction = connection.BeginTransaction(efContextSettings.IsolationLevel);
            } else {
                transaction = null;
            }
            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseSqlite(connection as SqliteConnection);
            options = builder.Options;
        }


        public static string FixSqliteConnectionString(string connectionString, IsolationLevel? isolationLevel) {
            //fix Sqlite
            var cxnString = connectionString;
            if (isolationLevel == IsolationLevel.ReadUncommitted &&
                !Regex.IsMatch(connectionString, "cache\\s*=\\s*shared", RegexOptions.IgnoreCase))
                cxnString = (connectionString + ";cache=shared").Replace(";;", ";");

            return cxnString;

        }

    }
}
