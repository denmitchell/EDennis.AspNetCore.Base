using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbConnectionManager {



        public static DbConnection<TContext> GetDbConnection<TContext>(DbContextInterceptorSettings<TContext> settings)
            where TContext : DbContext {

            return (settings.DatabaseProvider) switch
            {
                DatabaseProvider.SqlServer => GetSqlServerDbConnection(settings),
                DatabaseProvider.Sqlite => GetSqliteDbConnection(settings),
                DatabaseProvider.InMemory => GetInMemoryDbConnection<TContext>(),
                _ => null,
            };
        }


        public static DbContextOptionsBuilder ConfigureDbContextOptionsBuilder<TContext>(DbContextOptionsBuilder builder, DbContextSettings<TContext> settings)
            where TContext : DbContext {

            return (settings.DatabaseProvider) switch
            {
                DatabaseProvider.SqlServer => builder.UseSqlServer(settings.ConnectionString),
                DatabaseProvider.Sqlite => builder.UseSqlite(settings.ConnectionString),
                DatabaseProvider.InMemory => builder.UseInMemoryDatabase(Guid.NewGuid().ToString()),
                _ => null,
            };

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


        public static DbConnection<TContext> GetSqlServerDbConnection<TContext>(DbContextInterceptorSettings<TContext> settings)
            where TContext : DbContext {

            var dbConnection = new DbConnection<TContext>();
            var builder = new DbContextOptionsBuilder<TContext>();

            dbConnection.IDbConnection = new SqlConnection(settings.ConnectionString);
            dbConnection.IDbConnection.Open();
            dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(settings.IsolationLevel);

            builder.UseSqlite(dbConnection.IDbConnection as SqlConnection);
            dbConnection.DbContextOptionsBuilder = builder;
            return dbConnection;
        }

        public static DbConnection<TContext> GetSqliteDbConnection<TContext>(DbContextInterceptorSettings<TContext> settings)
            where TContext : DbContext {

            var dbConnection = new DbConnection<TContext>();
            var builder = new DbContextOptionsBuilder<TContext>();

            dbConnection.IDbConnection = new SqlConnection(FixSqliteConnectionString(settings.ConnectionString, settings.IsolationLevel));
            dbConnection.IDbConnection.Open();
            dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(settings.IsolationLevel);

            builder.UseSqlite(dbConnection.IDbConnection as SqlConnection);
            dbConnection.DbContextOptionsBuilder = builder;
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
