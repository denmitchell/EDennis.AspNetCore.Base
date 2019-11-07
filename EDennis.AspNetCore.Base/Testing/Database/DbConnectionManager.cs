using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace EDennis.AspNetCore.Base.Testing {
    public class DbConnectionManager {




        public static DbConnection<TContext> GetDbConnection<TContext>(EFContext efContextSettings)
            where TContext : DbContext {

            return (efContextSettings.ProviderName.ToLower()) switch
            {
                "sqlserver" => GetSqlServerDbConnection<TContext>(efContextSettings),
                "sqlite" => GetSqliteDbConnection<TContext>(efContextSettings),
                "inmemory" => GetInMemoryDbConnection<TContext>(),
                _ => null,
            };
        }



        public static DbConnection<TContext> GetInMemoryDbConnection<TContext>()
            where TContext: DbContext{

            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            return
                new DbConnection<TContext> {
                    DbContextOptions = builder.Options,
                    IDbConnection = null,
                    IDbTransaction = null
                };

        }

        public static DbConnection<TContext> GetSqlServerDbConnection<TContext>(EFContext efContextSettings)
            where TContext: DbContext{

            var dbConnection = new DbConnection<TContext> {
                IDbConnection = new SqlConnection(efContextSettings.ConnectionString)
            };
            dbConnection.IDbConnection.Open();

            if (efContextSettings.TransactionType == TransactionType.Rollback)
                dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(efContextSettings.IsolationLevel);

            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseSqlServer(dbConnection.IDbConnection as SqlConnection);
            dbConnection.DbContextOptions = builder.Options;

            return dbConnection;
        }

        public static DbConnection<TContext> GetSqliteDbConnection<TContext>(EFContext efContextSettings)
            where TContext : DbContext {

            var connectionString = FixSqliteConnectionString(efContextSettings.ConnectionString, efContextSettings.IsolationLevel);

            var dbConnection = new DbConnection<TContext> {
                IDbConnection = new SqliteConnection(connectionString)
            };
            dbConnection.IDbConnection.Open();

            if (efContextSettings.TransactionType == TransactionType.Rollback)
                dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(efContextSettings.IsolationLevel);

            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseSqlite(dbConnection.IDbConnection as SqliteConnection);
            dbConnection.DbContextOptions = builder.Options;

            return dbConnection;
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
