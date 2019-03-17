using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public abstract class ReadonlyRepo<TEntity, TContext> : IRepo
            where TEntity : class, new()
            where TContext : DbContext {


        public TContext Context { get; set; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public ReadonlyRepo(TContext context) {
            Context = context;
        }


        /// <summary>
        /// Provides direct access to the Query property of the context,
        /// allowing any query to be constructed via Linq expression
        /// </summary>
        public IQueryable<TEntity> Query { get => Context.Query<TEntity>(); }



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
        /// <typeparam name="T">The type of object to return</typeparam>
        /// <param name="sql">Valid SQL SELECT statement returning a scalar</param>
        /// <returns></returns>
        public virtual T GetScalarFromSql<T>(string sql) {
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            T result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<T>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<T>(sql);
            }
            return result;
        }


        /// <summary>
        /// Asynchronouely retrieves a scalar value from the database
        /// using the provided SQL SELECT statement
        /// </summary>
        /// <typeparam name="T">The type of object to return</typeparam>
        /// <param name="sql">Valid SQL SELECT statement returning a scalar</param>
        /// <returns></returns>
        public virtual async Task<T> GetScalarFromSqlAsync<T>(string sql) {
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            T result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<T>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<T>(sql);
            }
            return result;
        }



        /// <summary>
        /// Retrieves raw JSON from SQL Server using the
        /// provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public virtual string GetFromJsonSql(string fromJsonSql) {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<string>(sql);
            }
            return result;
        }


        /// <summary>
        /// Asynchronously retrieves raw JSON from SQL Server 
        /// using the provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public virtual async Task<string> GetFromJsonSqlAsync(string fromJsonSql) {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<string>(sql);
            }
            return result;
        }


    }


}

