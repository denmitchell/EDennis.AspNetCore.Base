using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    public class RelationalRepo<TEntity, TContext> : Repo<TEntity, TContext>, IRelationalRepo<TEntity> 
        where TEntity : class, new()
        where TContext : DbContext {

        public RelationalRepo(TContext context,
            ScopeProperties scopeProperties, 
            ILogger<Repo<TEntity, TContext>> logger) : base(context, scopeProperties, logger) {
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
        public virtual List<TEntity> GetFromSql(string sql){

            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<TEntity> result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
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
        public virtual async Task<List<TEntity>> GetFromSqlAsync(string sql){
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<TEntity> result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = (await cxn.QueryAsync<TEntity>(sql, transaction: dbTrans)).AsList();
            } else {
                result = (await cxn.QueryAsync<TEntity>(sql)).AsList();
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
        public virtual TScalarType GetScalarFromSql<TScalarType>(string sql){

            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            TScalarType result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
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
        public virtual async Task<TScalarType> GetScalarFromSqlAsync<TScalarType>(string sql){

            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            TScalarType result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<TScalarType>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<TScalarType>(sql);
            }
            return result;

        }




    }
}
