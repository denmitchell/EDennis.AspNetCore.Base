using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface IRelationalDbContext<TContext>
        where TContext : DbContext {
        DatabaseFacade Database { get; }
    }

    public static class IRelationalDbContextExtensions {


        /// <summary>
        /// Gets a list of TEntity using the provided
        /// SQL select statement.  Note: because this is
        /// a read-only query, Entity Framework will 
        /// throw an error if you attempt to perform
        /// a write operation (e.g, INSERT, UPDATE, or DELETE)
        /// </summary>
        /// <param name="sql">A valid SQL SELECT statement</param>
        /// <returns></returns>
        public static List<TEntity> GetListFromSql<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string sql)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<TEntity> result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.Query<TEntity>(sql, transaction: dbTrans).AsList();
            } else {
                result = cxn.Query<TEntity>(sql).AsList();
            }

            return result;
        }


        /// <summary>
        /// Asynchronously gets a list of TEntity using the provided
        /// SQL select statement.  Note: because this is
        /// a read-only query, Entity Framework will 
        /// throw an error if you attempt to perform
        /// a write operation (e.g, INSERT, UPDATE, or DELETE)
        /// </summary>
        /// <param name="sql">A valid SQL SELECT statement</param>
        /// <returns></returns>
        public static async Task<List<TEntity>> GetListFromSqlAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string sql)
            where TContext : DbContext {
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<TEntity> result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = (await cxn.QueryAsync<TEntity>(sql, transaction: dbTrans)).AsList();
            } else {
                result = (await cxn.QueryAsync<TEntity>(sql)).AsList();
            }
            return result;

        }


        /// <summary>
        /// Gets a list of TEntity using the provided
        /// SQL select statement.  Note: because this is
        /// a read-only query, Entity Framework will 
        /// throw an error if you attempt to perform
        /// a write operation (e.g, INSERT, UPDATE, or DELETE)
        /// </summary>
        /// <param name="sql">A valid SQL SELECT statement</param>
        /// <returns></returns>
        public static TEntity GetSingleFromSql<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string sql)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            TEntity result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.QuerySingle<TEntity>(sql, transaction: dbTrans);
            } else {
                result = cxn.QuerySingle<TEntity>(sql);
            }

            return result;
        }


        /// <summary>
        /// Asynchronously gets a list of TEntity using the provided
        /// SQL select statement.  Note: because this is
        /// a read-only query, Entity Framework will 
        /// throw an error if you attempt to perform
        /// a write operation (e.g, INSERT, UPDATE, or DELETE)
        /// </summary>
        /// <param name="sql">A valid SQL SELECT statement</param>
        /// <returns></returns>
        public static async Task<TEntity> GetSingleFromSqlAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string sql)
            where TContext : DbContext {
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            TEntity result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = (await cxn.QuerySingleAsync<TEntity>(sql, transaction: dbTrans));
            } else {
                result = (await cxn.QuerySingleAsync<TEntity>(sql));
            }
            return result;

        }


        /// <summary>
        /// Retrieves a scalar value from the database using
        /// the provided SQL SELECT statement
        /// </summary>
        /// <typeparam name="TScalarType">The type of object to return</typeparam>
        /// <param name="sql">Valid SQL SELECT statement returning a scalar</param>
        /// <returns></returns>
        public static TScalarType GetScalarFromSql<TContext, TScalarType>(this ISqlServerDbContext<TContext> context, string sql)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            TScalarType result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<TScalarType>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<TScalarType>(sql);
            }
            return result;

        }


        /// <summary>
        /// Asynchronously retrieves a scalar value from the database
        /// using the provided SQL SELECT statement
        /// </summary>
        /// <typeparam name="TScalarType">The type of object to return</typeparam>
        /// <param name="sql">Valid SQL SELECT statement returning a scalar</param>
        /// <returns></returns>
        public static async Task<TScalarType> GetScalarFromSqlAsync<TContext, TScalarType>(this ISqlServerDbContext<TContext> context, string sql)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            TScalarType result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<TScalarType>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<TScalarType>(sql);
            }
            return result;

        }


    }
}
