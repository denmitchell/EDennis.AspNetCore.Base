using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public class SqlRepo<TEntity, TContext>
            where TEntity : class, new()
            where TContext : DbContext {

        public TContext Context { get; set; }

        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public SqlRepo(TContext context) {
            Context = context;
        }


        /// <summary>
        /// Retrieves the entity with the provided primary key values
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>
        public virtual TEntity GetById(params object[] keyValues) {
            return Context.Find<TEntity>(keyValues);
        }


        /// <summary>
        /// Asychronously retrieves the entity with the provided primary key values.
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>
        public virtual async Task<TEntity> GetByIdAsync(params object[] keyValues) {
            return await Context.FindAsync<TEntity>(keyValues);
        }


        public IQueryable<TEntity> Query {
            get {
                return Context.Set<TEntity>()
                    .AsNoTracking();
            }
        }


        /// <summary>
        /// Determines if an object with the given primary key values
        /// exists in the context.
        /// </summary>
        /// <param name="keyValues">primary key values</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(params object[] keyValues) {
            var x = await Context.FindAsync<TEntity>(keyValues);
            var exists = (x != null);
            return exists;
        }


        /// <summary>
        /// Determines if an object with the given primary key values
        /// exists in the context.
        /// </summary>
        /// <param name="keyValues">primary key values</param>
        /// <returns>true if an entity with the provided keys exists</returns>
        public bool Exists(params object[] keyValues) {
            var x = Context.Find<TEntity>(keyValues);
            var exists = (x != null);
            return exists;
        }






        /// <summary>
        /// Creates a new entity from the provided input
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity</returns>
        public virtual TEntity Create(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot create a null {entity.GetType().Name}");

            Context.Entry(entity).State = EntityState.Added;
            Context.SaveChanges();
            return entity;
        }


        /// <summary>
        /// Asynchronously creates a new entity from the provided input
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot create a null {entity.GetType().Name}");

            Context.Entry(entity).State = EntityState.Added;
            await Context.SaveChangesAsync();
            return entity;
        }


        /// <summary>
        /// Updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual TEntity Update(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            Context.SafeAttach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();

            return entity;
        }


        /// <summary>
        /// Asynchronously updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity) {

            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            Context.SafeAttach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// Deletes the provided entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public virtual void Delete(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot delete a null {entity.GetType().Name}");

            Context.SafeAttach(entity);
            Context.Remove(entity);
            Context.SaveChanges();
        }

        /// <summary>
        /// Asychronously deletes the provided entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public virtual async Task DeleteAsync(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot delete a null {entity.GetType().Name}");

            Context.SafeAttach(entity);
            Context.Remove(entity);
            await Context.SaveChangesAsync();
        }



        /// <summary>
        /// Deletes the provided entity after updating it first.
        /// This is useful for scenarios in which the user id must
        /// be associated with the delete record.
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public virtual void DeleteUpdate(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot delete a null {entity.GetType().Name}");

            Context.SafeAttach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();

            Context.Remove(entity);
            Context.SaveChanges();
        }

        /// <summary>
        /// Asychronously deletes the provided entity after
        /// updating it first.
        /// This is useful for scenarios in which the user id must
        /// be associated with the delete record.
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public virtual async Task DeleteUpdateAsync(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot delete a null {entity.GetType().Name}");

            Context.SafeAttach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            Context.Remove(entity);
            await Context.SaveChangesAsync();
        }



        /// <summary>
        /// Deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual void Delete(params object[] keyValues) {
            TEntity entity = Context.Find<TEntity>(keyValues);
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot find {new TEntity().GetType().Name} object with key value = {PrintKeys(keyValues)}");

            Delete(entity);
        }

        /// <summary>
        /// Asynchrously deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual async Task DeleteAsync(params object[] keyValues) {
            TEntity entity = await Context.FindAsync<TEntity>(keyValues);
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot find {new TEntity().GetType().Name} object with key value = {PrintKeys(keyValues)}");

            await DeleteAsync(entity);
        }

        private string PrintKeys(params object[] keyValues) {
            return "[" + string.Join(",", keyValues) + "]";
        }


        /// <summary>
        /// Retrieves a page of all records defined by the provided SQL and parameters
        /// </summary>
        /// <param name="sql">Valid SQL select or exec stored procedure</param>
        /// <param name="pageNumber">The target result page</param>
        /// <param name="pageSize">The number of record per page</param>
        /// <param name="parameters">an array of parameters for the SQL statement</param>
        /// <returns>A list of all TEntity objects</returns>
        public virtual List<TEntity> GetFromSql(string sql, int pageNumber = 1, int pageSize = 10000, bool asNoTracking = true, params object[] parameters) {
            var qry = Context.Set<TEntity>().FromSql(sql, parameters);
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
        public virtual async Task<List<TEntity>> GetFromSqlAsync(string sql, int pageNumber = 1, int pageSize = 10000, bool asNoTracking = true, params object[] parameters) {
            var qry = Context.Set<TEntity>().FromSql(sql, parameters);
            if (asNoTracking)
                qry = qry.AsNoTracking();
            return await qry
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public virtual List<T> GetFromDapper<T>(string sql) {
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<T> result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.Query<T>(sql, transaction: dbTrans).AsList();
            } else {
                result = cxn.Query<T>(sql).AsList();
            }
            return result;
        }


        public virtual async Task<List<T>> GetFromDapperAsync<T>(string sql) {
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<T> result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = (await cxn.QueryAsync<T>(sql, transaction: dbTrans)).AsList();
            } else {
                result = (await cxn.QueryAsync<T>(sql)).AsList();
            }
            return result;
        }


        public virtual T GetScalarFromDapper<T>(string sql) {
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


        public virtual async Task<T> GetScalarFromDapperAsync<T>(string sql) {
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



        public virtual string GetFromJsonSql(string fromJsonSql) {
            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            return GetScalarFromDapper<string>(sql);
        }


        public virtual async Task<string> GetFromJsonSqlAsync(string fromJsonSql) {
            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            return await GetScalarFromDapperAsync<string>(sql);
        }


    }


}

