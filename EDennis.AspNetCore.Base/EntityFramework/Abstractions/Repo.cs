using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public class Repo<TEntity, TContext> : IRepo<TEntity, TContext>
            where TEntity : class, new()
            where TContext : DbContext {


        /// <summary>
        /// Mapping for SysUser, SysUserNext, SysCreate, SysStart, and SysEnd 
        /// -- when present.  Important Notes: 
        /// <para>For EntityFrameworkTemporalRepos, all of the 
        /// above properties, except SysCreate, must be present -- either with the specified names
        /// of as other properties with attributes (e.g., [SysStart]).
        /// </para>
        /// <para>
        /// By default, a property is deemed to be application-managed 
        /// special properties when either it matches in name or 
        /// attribute to one of the above-named properties.  The exceptions
        /// are SysStart and SysEnd, which for legacy reasons will be
        /// treated as database-managed in non-temporal repos. 
        /// and (b) 
        /// </para>
        /// </summary>
        protected static Dictionary<string, PropertyInfo> specialProperties;


        /// <summary>
        /// Statically builds mapping for SysUser, SysUserNext, SysStart, and 
        /// SysEnd -- when present;
        /// </summary>
        static Repo() {
            specialProperties = GetSpecialProperties<TEntity>();
        }



        private static Dictionary<string, PropertyInfo> GetSpecialProperties<T>() {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            var isEfTemporal = typeof(TemporalRepo<,,>).IsAssignableFrom(type);

            var attTypes = new Dictionary<Type,bool>{
                { typeof(SysUserAttribute), true },
                { typeof(SysUserNextAttribute), true },
                { typeof(SysStartAttribute), true },
                { typeof(SysEndAttribute), true },
                { typeof(SysCreatedAttribute), false }
                };

            var specialProps = new Dictionary<string, PropertyInfo>();
            var props = typeof(T).GetProperties();
            foreach (var attType in attTypes.Keys) {
                var name = attType.Name.Replace("Attribute", "");
                var prop = props.FirstOrDefault(
                    p => p.Name == name
                    || Attribute.IsDefined(p, attType));
                if (isEfTemporal && attTypes[attType] && prop == default)
                    throw new ApplicationException($"Application does not define property or attribute {name} for temporal repo of type {type.Name}");
                if (prop != null) {
                    if (Attribute.IsDefined(prop, attType))
                        specialProperties.Add(name, prop);
                    else if (isEfTemporal || (name != "SysStart" && name != "SysEnd"))
                        specialProperties.Add(name, prop);
                }
            }
            return specialProps;
        }


        protected void SetPropertyValue(TEntity entity, string property, object newValue, bool onlyIfNull = false) {
            if (onlyIfNull == false || entity.GetType().GetProperties().Any(p => p.Name == property))
                entity.GetType().GetProperty(property).SetValue(entity, newValue, null);
        }


        protected void SetSpecialPropertyValue(TEntity entity, string property, object newValue, bool onlyIfNull = false, bool optional = true) {
            if (specialProperties.ContainsKey(property) || !optional) {
                var prop = GetSpecialProperty(property);
                if (!onlyIfNull || prop.GetValue(entity) == null)
                    prop.SetValue(entity, newValue, null);
            }
        }

        protected void SetSpecialPropertyValueDynamic(dynamic partialEntity, string property, object newValue, bool onlyIfNull = false, bool optional = true) {
            if (specialProperties.ContainsKey(property) || !optional) {
                var prop = GetSpecialProperty(property);
                var dynProp = ((Type)partialEntity).GetType().GetProperties().FirstOrDefault(p => p.Name == prop.Name);
                if (!onlyIfNull || dynProp == null || dynProp.GetValue(partialEntity) == null)
                    dynProp.SetValue(partialEntity, newValue, null);
            }
        }


        protected object GetSpecialPropertyValue(TEntity entity, string property) {
            var prop = GetSpecialProperty(property);
            return prop.GetValue(entity);
        }

        protected PropertyInfo GetSpecialProperty(string property) {
            if (specialProperties.ContainsKey(property)) {
                return specialProperties[property];
            } else {
                throw new ApplicationException($"{typeof(TEntity).Name} does not contain a property named {property} or a property with a [{property}] attribute.");
            }
        }




        protected static string PrintKeys(params object[] keyValues) {
            return "[" + string.Join(",", keyValues) + "]";
        }



        /// <summary>
        /// Lifecycle method, called before SaveChanges in Create methods.
        /// if this method returns false, then bypass save operation
        /// </summary>
        public virtual bool BeforeCreate(TEntity entity) { return true; }


        /// <summary>
        /// Lifecycle method, called after SaveChanges in Create methods
        /// </summary>
        public virtual void AfterCreate(TEntity entity) { return; }


        /// <summary>
        /// Lifecycle method, called before SaveChanges in Update methods.
        /// if this method returns false, then bypass save operation
        /// </summary>
        public virtual bool BeforeUpdate(dynamic entityOrPartialEntity, params object[] keyValues) { return true; }


        /// <summary>
        /// Lifecycle method, called after SaveChanges in Update methods
        /// </summary>
        public virtual void AfterUpdate(dynamic entityOrPartialEntity, params object[] keyValues) { return; }


        /// <summary>
        /// Lifecycle method, called before SaveChanges in Delete methods
        /// if this method returns false, then bypass save operation
        /// </summary>
        public virtual bool BeforeDelete(params object[] keyValues) { return true; }


        /// <summary>
        /// Lifecycle method, called after SaveChanges in Delete methods
        /// </summary>
        public virtual void AfterDelete(params object[] keyValues) { return; }



        public TContext Context { get; set; }
        public ScopeProperties ScopeProperties { get; set; }

        public ILogger Logger { get; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public Repo(TContext context, ScopeProperties scopeProperties,
            ILogger<Repo<TEntity, TContext>> logger) {

            Context = context;
            ScopeProperties = scopeProperties;
            Logger = logger;

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



        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>
        public virtual List<dynamic> GetFromDynamicLinq(
                string where = null,
                string orderBy = null,
                string select = null,
                int? skip = null,
                int? take = null) {

            IQueryable qry = Query;

            if (where != null)
                qry = qry.Where(where);
            if (orderBy != null)
                qry = qry.OrderBy(orderBy);
            if (select != null)
                qry = qry.Select(select);
            if (skip != null)
                qry = qry.Skip(skip.Value);
            if (take != null)
                qry = qry.Take(take.Value);

            return qry.ToDynamicList();

        }


        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>
        public virtual async Task<List<dynamic>> GetFromDynamicLinqAsync(
                string where = null,
                string orderBy = null,
                string select = null,
                int? skip = null,
                int? take = null) {

            IQueryable qry = Query;

            if (where != null)
                qry = qry.Where(where);
            if (orderBy != null)
                qry = qry.OrderBy(orderBy);
            if (select != null)
                qry = qry.Select(select);
            if (skip != null)
                qry = qry.Skip(skip.Value);
            if (take != null)
                qry = qry.Take(take.Value);

            return await qry.ToDynamicListAsync();

        }


        /// <summary>
        /// Provides direct access to the Query property of the context,
        /// allowing any query to be constructed via Linq expression
        /// </summary>
        public virtual IQueryable<TEntity> Query { get => Context.Set<TEntity>().AsNoTracking(); }


        /// <summary>
        /// Determines if an object with the given primary key values
        /// exists in the context.
        /// </summary>
        /// <param name="keyValues">primary key values</param>
        /// <returns>true if an entity with the provided keys exists</returns>
        public virtual bool Exists(params object[] keyValues) {
            var entity = Context.Find<TEntity>(keyValues);
            if (entity != null)
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
        public virtual async Task<bool> ExistsAsync(params object[] keyValues) {
            var entity = await Context.FindAsync<TEntity>(keyValues);
            if (entity != null)
                Context.Entry(entity).State = EntityState.Detached;
            var exists = (entity != null);
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

            SetSpecialPropertyValue(entity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValue(entity, "SysStart", DateTime.Now, true, true);
            SetSpecialPropertyValue(entity, "SysCreated", DateTime.Now, true, true);

            if (BeforeCreate(entity)) {
                Context.Add(entity);
                Context.SaveChanges();
                AfterCreate(entity);
                return entity;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeCreate() returning false", ScopeProperties?.User ?? "");
                return null;
            }

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

            SetSpecialPropertyValue(entity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValue(entity, "SysStart", DateTime.Now, true, true);
            SetSpecialPropertyValue(entity, "SysCreated", DateTime.Now, true, true);

            if (BeforeCreate(entity)) {
                Context.Add(entity);
                await Context.SaveChangesAsync();
                AfterCreate(entity);
                return entity;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeCreate() returning false", ScopeProperties?.User ?? "");
                return null;
            }

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

            //retrieve the existing entity
            var existing = Context.Find<TEntity>(keyValues);

            SetSpecialPropertyValue(entity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValue(entity, "SysStart", DateTime.Now, true, true);

            //copy property values from entity to existing
            Context.Entry(existing).CurrentValues.SetValues(entity);

            if (BeforeUpdate(entity, keyValues)) {
                Context.Update(entity);
                Context.SaveChanges();
                AfterUpdate(existing, keyValues);
                return existing;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeUpdate() returning false", ScopeProperties?.User ?? "");
                return null;
            }
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

            //retrieve the existing entity
            var existing = await Context.FindAsync<TEntity>(keyValues);

            SetSpecialPropertyValue(entity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValue(entity, "SysStart", DateTime.Now, true, true);

            //copy property values from entity to existing
            Context.Entry(existing).CurrentValues.SetValues(entity);

            if (BeforeUpdate(entity, keyValues)) {
                Context.Update(entity);
                await Context.SaveChangesAsync();
                AfterUpdate(existing, keyValues);
                return existing;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeUpdate() returning false", ScopeProperties?.User ?? "");
                return null;
            }
        }





        public virtual TEntity Update(dynamic partialEntity, params object[] keyValues) {
            if (partialEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            SetSpecialPropertyValueDynamic(partialEntity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValueDynamic(partialEntity, "SysStart", DateTime.Now, true, true);

            //retrieve the existing entity
            var existing = Context.Find<TEntity>(keyValues);

            //copy property values from entity to existing
            DynamicExtensions.Populate<TEntity>(existing, partialEntity);

            if (BeforeUpdate(partialEntity, keyValues)) {
                Context.Update(existing);
                Context.SaveChanges();
                AfterUpdate(existing, keyValues);
                return existing; //updated entity
            } else {
                Logger.LogDebug("For user {User}, call to BeforeUpdate() returning false", ScopeProperties?.User ?? "");
                return null;
            }

        }



        public virtual async Task<TEntity> UpdateAsync(dynamic partialEntity, params object[] keyValues) {
            if (partialEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            SetSpecialPropertyValueDynamic(partialEntity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValueDynamic(partialEntity, "SysStart", DateTime.Now, true, true);

            //retrieve the existing entity
            var existing = await Context.FindAsync<TEntity>(keyValues);

            //copy property values from entity to existing
            DynamicExtensions.Populate<TEntity>(existing, partialEntity);

            if (BeforeUpdate(partialEntity, keyValues)) {
                Context.Update(existing);
                await Context.SaveChangesAsync();
                AfterUpdate(existing, keyValues);
                return existing; //updated entity
            } else {
                Logger.LogDebug("For user {User}, call to BeforeUpdate() returning false", ScopeProperties?.User ?? "");
                return null;
            }
        }




        /// <summary>
        /// Deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual void Delete(params object[] keyValues) {

            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {typeof(TEntity).Name} object with key value = {PrintKeys(keyValues)}");

            if (BeforeDelete(keyValues)) {
                Context.Remove(existing);
                Context.SaveChanges();
                AfterDelete(keyValues);
            } else {
                Logger.LogDebug("For user {User}, call to BeforeDelete() returning false", ScopeProperties?.User ?? "");
            }

        }



        /// <summary>
        /// Asynchrously deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual async Task DeleteAsync(params object[] keyValues) {
            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {typeof(TEntity).Name} object with key value = {PrintKeys(keyValues)}");

            if (BeforeDelete(keyValues)) {
                Context.Remove(existing);
                await Context.SaveChangesAsync();
                AfterDelete(keyValues);
                return;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeDelete() returning false", ScopeProperties?.User ?? "");
                return;
            }
        }







    }


}

