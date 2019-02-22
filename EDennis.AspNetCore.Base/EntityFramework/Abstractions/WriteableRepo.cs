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
    public abstract class WriteableRepo<TEntity, TContext> : ReadonlyRepo<TEntity,TContext>
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext {


        public string SysUser { get; set; }

        public ScopeProperties ScopeProperties { get; set; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public WriteableRepo(TContext context, ScopeProperties scopeProperties) :
            base (context){
            ScopeProperties = scopeProperties;
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

            entity.SysUser = ScopeProperties.User;
            Context.Add(entity);
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

            entity.SysUser = ScopeProperties.User;
            Context.Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }


        /// <summary>
        /// Updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual TEntity Update(TEntity entity, params object[] keyValues) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            var existing = Context.Find<TEntity>(keyValues);

            entity.SysUser = ScopeProperties.User;
            Context.Update(entity);
            Context.SaveChanges();

            return entity;
        }


        /// <summary>
        /// Asynchronously updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, params object[] keyValues) {

            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            var existing = await Context.FindAsync<TEntity>(keyValues);

            entity.SysUser = ScopeProperties.User;
            Context.Update(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// Deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual void Delete(params object[] keyValues) {

            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {new TEntity().GetType().Name} object with key value = {PrintKeys(keyValues)}");


            Context.Remove(existing);
            Context.SaveChanges();

        }

        /// <summary>
        /// Asynchrously deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual async Task DeleteAsync(params object[] keyValues) {
            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {new TEntity().GetType().Name} object with key value = {PrintKeys(keyValues)}");

            Context.Remove(existing);
            await Context.SaveChangesAsync();
        }



        private string PrintKeys(params object[] keyValues) {
            return "[" + string.Join(",", keyValues) + "]";
        }





    }


}

