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
    public static class RepoSqlExtensions {
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


        public static List<TEntity> GetFromDapper<TEntity, TContext>
                    (this ReadonlyRepo<TEntity, TContext> repo, string sql)
                        where TEntity : class, new()
            where TContext : DbContext {
            var cxn = repo.Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<TEntity> result;
            if (repo.Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.Query<TEntity>(sql, transaction: dbTrans).AsList();
            } else {
                result = cxn.Query<TEntity>(sql).AsList();
            }
            return result;
        }


        public static async Task<List<TEntity>> GetFromDapperAsync<TEntity, TContext>
                    (this ReadonlyRepo<TEntity, TContext> repo, string sql)
                        where TEntity : class, new()
            where TContext : DbContext {
            var cxn = repo.Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<TEntity> result;
            if (repo.Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = (await cxn.QueryAsync<TEntity>(sql, transaction: dbTrans)).AsList();
            } else {
                result = (await cxn.QueryAsync<TEntity>(sql)).AsList();
            }
            return result;
        }


        public static TEntity GetScalarFromDapper<TEntity, TContext>
                    (this ReadonlyRepo<TEntity, TContext> repo, string sql)
                        where TEntity : class, new()
            where TContext : DbContext {
            var cxn = repo.Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            TEntity result;
            if (repo.Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<TEntity>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<TEntity>(sql);
            }
            return result;
        }


        public static async Task<TEntity> GetScalarFromDapperAsync<TEntity, TContext>
                    (this ReadonlyRepo<TEntity, TContext> repo, string sql)
                        where TEntity : class, new()
            where TContext : DbContext {
            var cxn = repo.Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            TEntity result;
            if (repo.Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<TEntity>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<TEntity>(sql);
            }
            return result;
        }



        public static string GetFromJsonSql<TEntity, TContext>
                    (this ReadonlyRepo<TEntity, TContext> repo, string fromJsonSql)
                        where TEntity : class, new()
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = repo.Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (repo.Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<string>(sql);
            }
            return result;
        }


        public static async Task<string> GetFromJsonSqlAsync<TEntity, TContext>
                    (this ReadonlyRepo<TEntity, TContext> repo, string fromJsonSql)
                        where TEntity : class, new()
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = repo.Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (repo.Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<string>(sql);
            }
            return result;
        }


    }
}
