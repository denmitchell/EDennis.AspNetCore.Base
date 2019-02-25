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
        /// Retrieves the entity with the provided primary key values
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>
        public virtual TEntity GetById(params object[] keyValues) {
            var entity = Context.Find<TEntity>(keyValues);
            Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }


        /// <summary>
        /// Asychronously retrieves the entity with the provided primary key values.
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>
        public virtual async Task<TEntity> GetByIdAsync(params object[] keyValues) {
            var entity = await Context.FindAsync<TEntity>(keyValues);
            Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }


        public IQueryable<TEntity> Query {
            get {
                return Context.Query<TEntity>();
            }
        }


        /// <summary>
        /// Determines if an object with the given primary key values
        /// exists in the context.
        /// </summary>
        /// <param name="keyValues">primary key values</param>
        /// <returns>true if an entity with the provided keys exists</returns>
        public bool Exists(params object[] keyValues) {
            var entity = Context.Find<TEntity>(keyValues);
            Context.Entry(entity).State = EntityState.Detached;
            var exists = (entity != null);
            return exists;
        }

        
        /// <summary>
        /// Determines if an object with the given primary key values
        /// exists in the context.
        /// </summary>
        /// <param name="keyValues">primary key values</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(params object[] keyValues) {
            var entity = await Context.FindAsync<TEntity>(keyValues);
            Context.Entry(entity).State = EntityState.Detached;
            var exists = (entity != null);
            return exists;
        }




        public List<TEntity> GetFromSql(string sql){
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


        public async Task<List<TEntity>> GetFromSqlAsync(string sql){
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


        public T GetScalarFromSql<T>(string sql) {
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


        public async Task<T> GetScalarFromSqlAsync<T>(string sql) {
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



        public string GetFromJsonSql(string fromJsonSql) {

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


        public async Task<string> GetFromJsonSqlAsync(string fromJsonSql) {

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



        protected string PrintKeys(params object[] keyValues) {
            return "[" + string.Join(",", keyValues) + "]";
        }


    }


}

