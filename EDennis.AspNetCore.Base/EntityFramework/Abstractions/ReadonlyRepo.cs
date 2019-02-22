using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Http;
using System;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public abstract class ReadonlyRepo<TEntity, TContext>
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



    }


}

