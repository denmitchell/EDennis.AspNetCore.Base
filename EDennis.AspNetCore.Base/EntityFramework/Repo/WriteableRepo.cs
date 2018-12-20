using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    /// <see cref="QueryableRepo{TEntity, TContext}"/>
    public class WriteableRepo<TEntity, TContext> : IRepo, IDisposable
            where TEntity : class, new()
            where TContext : DbContextBase {

        protected TContext Context { get; set; }
        protected DbSet<TEntity> _dbset;

        //whether this repo has a testing instance of DbContextBase
        public bool IsTestingInstance { get; set; } = false;


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public WriteableRepo(TContext context) {
            Context = context;

            //get a reference to the DbSet
            _dbset = Context.Set<TEntity>();
        }


        /// <summary>
        /// Retrieves the entity with the provided primary key values
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>
        public virtual TEntity GetById(params object[] keyValues) {
            return _dbset.Find(keyValues);
        }


        /// <summary>
        /// Asychronously retrieves the entity with the provided primary key values.
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>
        public virtual async Task<TEntity> GetByIdAsync(params object[] keyValues) {
            return await _dbset.FindAsync(keyValues);
            //without _dbSet ...
            //return await Context.FindAsync<TEntity>(keyValues);
        }


        /// <summary>
        /// Retrieves a page of all records defined by the provided LINQ expression
        /// </summary>
        /// <param name="linqExpression">Valid LINQ expression</param>
        /// <param name="pageNumber">The target result page</param>
        /// <param name="pageSize">The number of record per page</param>
        /// <returns>A list of all TEntity objects</returns>
        public virtual List<TEntity> GetByLinq(Expression<Func<TEntity, bool>> linqExpression, int pageNumber, int pageSize) {
            return _dbset.Where(linqExpression)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            //without _dbSet ...
            //return Context.Set<TEntity>()
            //    .Where(linqExpression)
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();
        }


        /// <summary>
        /// Asynchronously retrieves a page of all records defined by the provided LINQ expression.
        /// </summary>
        /// <param name="linqExpression">Valid LINQ expression</param>
        /// <param name="pageNumber">The target result page</param>
        /// <param name="pageSize">The number of record per page</param>
        /// <returns>A list of all TEntity objects</returns>
        public virtual async Task<List<TEntity>> GetByLinqAsync(Expression<Func<TEntity, bool>> linqExpression, int pageNumber, int pageSize) {
            return await _dbset.Where(linqExpression)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



        /// <summary>
        /// Determines if an object with the given primary key values
        /// exists in the context.
        /// </summary>
        /// <param name="keyValues">primary key values</param>
        /// <returns></returns>
        public async Task<Boolean> ExistsAsync(params object[] keyValues) {
            var x = await _dbset.FindAsync(keyValues);
            var exists = (x != null);
            Context.Entry(x).State = EntityState.Detached;
            return exists;
            //return await context.Items.AnyAsync(i => i.ItemId == id);
        }


        /// <summary>
        /// Determines if an object with the given primary key values
        /// exists in the context.
        /// </summary>
        /// <param name="keyValues">primary key values</param>
        /// <returns>true if an entity with the provided keys exists</returns>
        public bool Exists(params object[] keyValues) {
            var x = _dbset.Find(keyValues);
            var exists = (x != null);
            Context.Entry(x).State = EntityState.Detached;
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
            //_dbset.Add(entity);
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

            _dbset.Add(entity);
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

            Context.Attach(entity);
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

            Context.Attach(entity);
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

            if (Context.Entry(entity).State == EntityState.Detached)
                _dbset.Attach(entity);

            _dbset.Remove(entity);
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

            if (Context.Entry(entity).State == EntityState.Detached)
                _dbset.Attach(entity);

            _dbset.Remove(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual void Delete(params object[] keyValues) {
            TEntity entity = _dbset.Find(keyValues);
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
            TEntity entity = _dbset.Find(keyValues);
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
        public virtual List<TEntity> GetFromSql(string sql, int pageNumber, int pageSize, params object[] parameters) {
            return _dbset.FromSql(sql, parameters)
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
        public virtual async Task<List<TEntity>> GetFromSqlAsync(string sql, int pageNumber, int pageSize, params object[] parameters) {
            return await _dbset.FromSql(sql, parameters)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    //rollback any outstanding transactions
                    if (IsTestingInstance)
                        if (Context.Database.CurrentTransaction != null) {
                            var cxn = Context.Database.GetDbConnection() as SqlConnection;
                            if (cxn.State == System.Data.ConnectionState.Open) {
                                Context.Database.RollbackTransaction();
                                if (Context.HasIdentities)
                                    cxn.ResetIdentities();
                                if (Context.HasSequences)
                                    cxn.ResetSequences();
                            }
                        }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

    }
}
