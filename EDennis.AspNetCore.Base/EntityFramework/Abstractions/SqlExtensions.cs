using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework.Sql {
    public static class SqlExtensions {
        /// <summary>
        /// Retrieves a page of all records defined by the provided SQL and parameters
        /// </summary>
        /// <param name="sql">Valid SQL select or exec stored procedure</param>
        /// <param name="pageNumber">The target result page</param>
        /// <param name="pageSize">The number of record per page</param>
        /// <param name="parameters">an array of parameters for the SQL statement</param>
        /// <returns>A list of all TEntity objects</returns>
        public static List<TEntity> GetFromSql<TEntity, TContext>
                    (this ReadonlyRepo<TEntity, TContext> repo, string sql, int pageNumber = 1, int pageSize = 10000, bool asNoTracking = true, params object[] parameters)
                        where TEntity : class, new()
            where TContext : DbContext {
            var qry = repo.Context.Set<TEntity>().FromSql(sql, parameters);
            if (asNoTracking)
                qry = qry.AsNoTracking();
            return qry
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        /// <summary>
        /// Retrieves a page of all records defined by the provided SQL and parameters
        /// </summary>
        /// <param name="sql">Valid SQL select or exec stored procedure</param>
        /// <param name="pageNumber">The target result page</param>
        /// <param name="pageSize">The number of record per page</param>
        /// <param name="parameters">an array of parameters for the SQL statement</param>
        /// <returns>A list of all TEntity objects</returns>
        public static async Task<List<TEntity>> GetFromSqlAsync<TEntity, TContext>
                    (this ReadonlyRepo<TEntity, TContext> repo, string sql, int pageNumber = 1, int pageSize = 10000, bool asNoTracking = true, params object[] parameters)
                        where TEntity : class, new()
            where TContext : DbContext {
            var qry = repo.Context.Set<TEntity>().FromSql(sql, parameters);
            if (asNoTracking)
                qry = qry.AsNoTracking();
            return await qry
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public static List<TEntity> GetFromSql<TEntity>
                    (this DbContext context, string sql)
                        where TEntity : class, new() {
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


        public static async Task<List<TEntity>> GetFromSqlAsync<TEntity>
                    (this DbContext context, string sql)
                        where TEntity : class, new() {
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


        public static T GetScalarFromSql<T>
                    (this DbContext context, string sql) {
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            T result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<T>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<T>(sql);
            }
            return result;
        }


        public static async Task<T> GetScalarFromSqlAsync<T>
                    (this DbContext context, string sql) {
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            T result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<T>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<T>(sql);
            }
            return result;
        }



        public static string GetFromJsonSql
                    (this DbContext context, string fromJsonSql){

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<string>(sql);
            }
            return result;
        }


        public static async Task<string> GetFromJsonSqlAsync
                    (this DbContext context, string fromJsonSql) {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<string>(sql);
            }
            return result;
        }


    }
}
