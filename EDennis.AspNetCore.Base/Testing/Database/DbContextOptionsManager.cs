using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base.Testing {
    public class DbContextOptionsManager<TContext>
                    where TContext : DbContext {

        private DbConnection<TContext> dbConnection;

        public DbContextOptionsManager<TContext> BuildOptions<TDbConnection>(
            string connectionString, IsolationLevel? isolationLevel)
            where TDbConnection : IDbConnection {
            dbConnection = GetDbContextOptions<TDbConnection>(connectionString, isolationLevel);
            return this;
        }

        public DbContextOptionsManager<TContext> BuildOptions(string databaseName) {
            dbConnection = GetDbContextOptions(databaseName);
            return this;
        }

        public DbContextOptionsManager<TContext> UpdateCache(string key, DbConnectionCache<TContext> cache) {
            if (cache.ContainsKey(key))
                cache.Remove(key);
            else
                cache.Add(key, dbConnection);
            return this;
        }

        public DbContextOptionsManager<TContext> UpdateProvider(
            DbContextOptionsProvider<TContext> dbContextOptionsProvider) {
            dbContextOptionsProvider.DbContextOptions = dbConnection.DbContextOptions;
            return this;
        }



        public static DbConnection<TContext> GetDbContextOptions<TDbConnection>(
            string connectionString, IsolationLevel? isolationLevel)
            where TDbConnection : IDbConnection {
            IDbConnection connection;
            IDbTransaction transaction = null;
            connection = (TDbConnection)Activator.CreateInstance(typeof(TDbConnection));
            connection.ConnectionString = connectionString;
            connectionString = FixSqliteConnectionString(connectionString, isolationLevel);
            connection.Open();
            if (isolationLevel != null) {
                if (connection is SqlConnection || (connection is SqliteConnection && isolationLevel == IsolationLevel.ReadUncommitted))
                    transaction = connection.BeginTransaction(isolationLevel.Value);
            }
            var builder = new DbContextOptionsBuilder<TContext>();
            if (connection is SqlConnection)
                builder.UseSqlServer(connection as SqlConnection);
            else if (connection is SqliteConnection) {
                builder.UseSqlite(connection as SqliteConnection);
            } else
                throw new ApplicationException($"GetDbContextOptions called without valid TDbConnection type parameter.  Supported types are SqlConnection and SqliteConnection");
            return
                new DbConnection<TContext> {
                    DbContextOptions = builder.Options,
                    IDbConnection = connection,
                    IDbTransaction = transaction
                };
        }


        public static DbConnection<TContext> GetDbContextOptions(string databaseName) {
            var options = new DbContextOptionsBuilder<TContext>()
                            .UseInMemoryDatabase(databaseName)
                            .Options;
            return new DbConnection<TContext> {
                DbContextOptions = options,
                IDbConnection = null,
                IDbTransaction = null
            };
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
