using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace EDennis.AspNetCore.Base.Testing {
    public class DbConnectionManager {




        public static DbConnection<TContext> GetDbConnection<TContext>(DbContextSettings<TContext> settings)
            where TContext : DbContext {

            return (settings.DatabaseProvider) switch
            {
                DatabaseProvider.SqlServer => GetSqlServerDbConnection<TContext>(settings),
                DatabaseProvider.Sqlite => GetSqliteDbConnection<TContext>(settings),
                DatabaseProvider.InMemory => GetInMemoryDbConnection<TContext>(),
                _ => null,
            };
        }

        public static void ConfigureDbContextOptionsBuilder<TContext>(DbContextOptionsBuilder builder, DbContextSettings<TContext> settings)
            where TContext : DbContext {

            switch (settings.DatabaseProvider) {
                case DatabaseProvider.SqlServer:
                    ConfigureDbContextOptionsBuilder_SqlServer(builder, settings);
                    break;
                case DatabaseProvider.Sqlite:
                    ConfigureDbContextOptionsBuilder_Sqlite(builder, settings);
                    break;
                case DatabaseProvider.InMemory:
                    ConfigureDbContextOptionsBuilder_InMemory(builder, settings);
                    break;
                default:
                    break;
            }

        }

        public static void ConfigureDbContextOptionsBuilder_SqlServer<TContext>(DbContextOptionsBuilder builder, DbContextSettings<TContext> settings)
            where TContext : DbContext {

            var dbConnection = new DbConnection<TContext>();
            if (settings.TransactionType == TransactionType.Rollback) {
                dbConnection.IDbConnection = new SqlConnection(settings.ConnectionString);
                dbConnection.IDbConnection.Open();
                dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(settings.IsolationLevel);
                builder.UseSqlServer(dbConnection.IDbConnection as SqlConnection);
            } else {
                builder.UseSqlServer(settings.ConnectionString);
            }

        }


        public static void ConfigureDbContextOptionsBuilder_Sqlite<TContext>(DbContextOptionsBuilder builder, DbContextSettings<TContext> settings)
            where TContext : DbContext {

            var connectionString = FixSqliteConnectionString(settings.ConnectionString, settings.IsolationLevel);
            var dbConnection = new DbConnection<TContext>();
            if (settings.TransactionType == TransactionType.Rollback) {
                dbConnection.IDbConnection = new SqlConnection(settings.ConnectionString);
                dbConnection.IDbConnection.Open();
                dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(settings.IsolationLevel);
                builder.UseSqlite(dbConnection.IDbConnection as SqlConnection);
            } else {
                builder.UseSqlite(settings.ConnectionString);
            }
        }


        public static void ConfigureDbContextOptionsBuilder_InMemory<TContext>(DbContextOptionsBuilder builder, DbContextSettings<TContext> settings)
            where TContext : DbContext {

            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }



        public static DbConnection<TContext> GetInMemoryDbConnection<TContext>()
                    where TContext : DbContext {

            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            return
                new DbConnection<TContext> {
                    DbContextOptionsBuilder = builder,
                    IDbConnection = null,
                    IDbTransaction = null
                };

        }

        public static DbConnection<TContext> GetSqlServerDbConnection<TContext>(DbContextSettings<TContext> settings)
            where TContext : DbContext {

            var dbConnection = new DbConnection<TContext>();
            var builder = new DbContextOptionsBuilder<TContext>();

            if (settings.TransactionType == TransactionType.Rollback) {
                dbConnection.IDbConnection = new SqlConnection(settings.ConnectionString);
                dbConnection.IDbConnection.Open();
                dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(settings.IsolationLevel);
                builder.UseSqlServer(dbConnection.IDbConnection as SqlConnection);
                dbConnection.DbContextOptionsBuilder = builder;
            } else {
                builder.UseSqlServer(settings.ConnectionString);
                dbConnection.DbContextOptionsBuilder = builder;
                return dbConnection;
            }

            return dbConnection;
        }

        public static DbConnection<TContext> GetSqliteDbConnection<TContext>(DbContextSettings<TContext> settings)
            where TContext : DbContext {

            var connectionString = FixSqliteConnectionString(settings.ConnectionString, settings.IsolationLevel);
            var dbConnection = new DbConnection<TContext>();
            var builder = new DbContextOptionsBuilder<TContext>();


            if (settings.TransactionType == TransactionType.Rollback) {
                dbConnection.IDbConnection = new SqliteConnection(settings.ConnectionString);
                dbConnection.IDbConnection.Open();
                dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(settings.IsolationLevel);
                builder.UseSqlServer(dbConnection.IDbConnection as SqliteConnection);
                dbConnection.DbContextOptionsBuilder = builder;
            } else {
                builder.UseSqlServer(settings.ConnectionString);
                dbConnection.DbContextOptionsBuilder = builder;
                return dbConnection;
            }

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
