using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public partial class Repo<TEntity, TContext> : IRepo<TEntity, TContext> where TEntity : class, IHasSysUser, IHasLongId, new()
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


        protected static string PrintKeys(params object[] keyValues) {
            return "[" + string.Join(",", keyValues) + "]";
        }



        /// <summary>
        /// Lifecycle method, called before SaveChanges in Create methods.
        /// if this method returns false, then bypass save operation
        /// </summary>
        public virtual bool BeforeCreate(TEntity inputEntity) { return true; }


        /// <summary>
        /// Lifecycle method, called after SaveChanges in Create methods
        /// </summary>
        public virtual void AfterCreate(TEntity inputEntity, TEntity resultEntity) {
        }


        /// <summary>
        /// Lifecycle method, called before SaveChanges in Update methods.
        /// if this method returns false, then bypass save operation
        /// </summary>
        public virtual bool BeforeUpdate(dynamic inputEntity, params object[] keyValues) { return true; }


        /// <summary>
        /// Lifecycle method, called after SaveChanges in Update methods
        /// </summary>
        public virtual void AfterUpdate(dynamic inputEntity, TEntity resultEntity, params object[] keyValues) { return; }


        /// <summary>
        /// Lifecycle method, called before SaveChanges in Delete methods
        /// if this method returns false, then bypass save operation
        /// </summary>
        public virtual bool BeforeDelete(params object[] keyValues) { return true; }


        /// <summary>
        /// Lifecycle method, called after SaveChanges in Delete methods
        /// </summary>
        public virtual void AfterDelete(TEntity deletedEntity, params object[] keyValues) { return; }



        public TContext Context { get; set; }
        public ScopeProperties ScopeProperties { get; set; }

        public ILogger Logger { get; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public Repo(TContext context,
            ScopeProperties scopeProperties,
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


        public virtual List<dynamic> GetFromDynamicLinq(DynamicLinqParameters parms) {
            return GetFromDynamicLinq(parms.Where, parms.OrderBy, parms.Select, parms.Skip, parms.Take);
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

        public virtual async Task<List<dynamic>> GetFromDynamicLinqAsync(DynamicLinqParameters parms) {
            return await GetFromDynamicLinqAsync(parms.Where, parms.OrderBy, parms.Select, parms.Skip, parms.Take);
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
        /// <param name="inputEntity">The entity to create</param>
        /// <returns>The created entity</returns>
        public virtual TEntity Create(TEntity inputEntity) {
            if (inputEntity == null)
                throw new MissingEntityException(
                    $"Cannot create a null {inputEntity.GetType().Name}");

            inputEntity.SysUser ??= ScopeProperties.User;

            if (BeforeCreate(inputEntity)) {
                var originalInputEntity = inputEntity.DeepClone();
                Context.Add(inputEntity);
                Context.SaveChanges();

                AfterCreate(originalInputEntity, inputEntity);
                return inputEntity;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeCreate() returning false", ScopeProperties?.User ?? "?");
                return null;
            }

        }

        /// <summary>
        /// Asynchronously creates a new entity from the provided input
        /// </summary>
        /// <param name="inputEntity">The entity to create</param>
        /// <returns>The created entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity inputEntity) {
            if (inputEntity == null)
                throw new MissingEntityException(
                    $"Cannot create a null {inputEntity.GetType().Name}");

            inputEntity.SysUser ??= ScopeProperties.User;

            if (BeforeCreate(inputEntity)) {
                var originalInputEntity = inputEntity.DeepClone();
                Context.Add(inputEntity);
                await Context.SaveChangesAsync();
                AfterCreate(originalInputEntity, inputEntity);
                return inputEntity;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeCreate() returning false", ScopeProperties?.User ?? "?");
                return null;
            }

        }




        /// <summary>
        /// Updates the provided entity
        /// </summary>
        /// <param name="inputEntity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual TEntity Update(TEntity inputEntity, params object[] keyValues) {
            if (inputEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {inputEntity.GetType().Name}");

            //retrieve the existing entity
            var resultEntity = Context.Find<TEntity>(keyValues);

            inputEntity.SysUser ??= ScopeProperties.User;

            //copy property values from entity to existing (resultEntity)
            Context.Entry(resultEntity).CurrentValues.SetValues(inputEntity);

            if (BeforeUpdate(inputEntity, keyValues)) {
                Context.Update(inputEntity);
                Context.SaveChanges();
                AfterUpdate(inputEntity, resultEntity, keyValues);
                return resultEntity;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeUpdate() returning false", ScopeProperties?.User ?? "?");
                return null;
            }
        }


        /// <summary>
        /// Asynchronously updates the provided entity
        /// </summary>
        /// <param name="inputEntity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity inputEntity, params object[] keyValues) {

            if (inputEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {inputEntity.GetType().Name}");

            //retrieve the existing entity (resultEntity)
            var existing = await Context.FindAsync<TEntity>(keyValues);

            inputEntity.SysUser ??= ScopeProperties.User;

            //copy property values from entity to existing
            Context.Entry(existing).CurrentValues.SetValues(inputEntity);

            if (BeforeUpdate(inputEntity, keyValues)) {
                Context.Update(inputEntity);
                await Context.SaveChangesAsync();
                AfterUpdate(inputEntity, existing, keyValues);
                return existing;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeUpdate() returning false", ScopeProperties?.User ?? "?");
                return null;
            }
        }






        public virtual TEntity Update(dynamic partialEntity, params object[] keyValues) {
            if (partialEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {typeof(TEntity).Name}");


            //retrieve the existing entity
            var existing = Context.Find<TEntity>(keyValues);

            partialEntity.SysUser ??= ScopeProperties.User;

            //copy property values from entity to existing
            DynamicExtensions.Populate<TEntity>(existing, partialEntity);

            if (BeforeUpdate(partialEntity, keyValues)) {
                Context.Update(existing);
                Context.SaveChanges();
                AfterUpdate(partialEntity, existing, keyValues);
                return existing; //updated entity
            } else {
                Logger.LogDebug("For user {User}, call to BeforeUpdate() returning false", ScopeProperties?.User ?? "?");
                return null;
            }

        }



        public virtual async Task<TEntity> UpdateAsync(dynamic partialEntity, params object[] keyValues) {
            if (partialEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            partialEntity.SysUser ??= ScopeProperties.User;

            //retrieve the existing entity
            var existing = await Context.FindAsync<TEntity>(keyValues);

            //copy property values from entity to existing
            DynamicExtensions.Populate<TEntity>(existing, partialEntity);

            if (BeforeUpdate(partialEntity, keyValues)) {
                Context.Update(existing);
                await Context.SaveChangesAsync();
                AfterUpdate(partialEntity, existing, keyValues);
                return existing; //updated entity
            } else {
                Logger.LogDebug("For user {User}, call to BeforeUpdate() returning false", ScopeProperties?.User ?? "?");
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
                AfterDelete(existing, keyValues);
            } else {
                Logger.LogDebug("For user {User}, call to BeforeDelete() returning false", ScopeProperties?.User ?? "?");
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
                AfterDelete(existing, keyValues);
                return;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeDelete() returning false", ScopeProperties?.User ?? "?");
                return;
            }
        }

        public Dictionary<string, long> GetNextCompoundIdBlock() {
            var dict = new Dictionary<string, long> {
                { "", Query.Max(x => x.Id) }
            };
            var dict2 = Query
                .GroupBy(x => x.SysUser)
                .Select(g => KeyValuePair.Create(g.Key, g.Max(x => x.Id)));
            foreach (var entry in dict2)
                dict.Add(entry.Key, entry.Value);
            return dict;
        }



        public virtual dynamic CheckPreState(string checkContext,
            DynamicLinqParameters preStateParameters,
            Func<dynamic, bool> isExpectedPreState) {
            var pre = GetFromDynamicLinq(preStateParameters);
            if (!isExpectedPreState(pre)) {
                var preJson = JsonSerializer.Serialize<dynamic>(pre);
                IEnumerable<KeyValuePair<string, object>> dict = new List<KeyValuePair<string, object>> {
                    KeyValuePair.Create("PreState",(object)preJson),
                };
                var ex = new ApplicationException($"{GetType().Name} pre-state check failed for {checkContext}");
                using (Logger.BeginScope(dict)) {
                    Logger.LogError(ex, "For user {User}, {Repo} pre-state check failed for {CheckContext}", ScopeProperties?.User ?? "?", GetType().Name, checkContext);
                }
                throw ex;
            }
            return pre;
        }



        public virtual async Task<dynamic> CheckPreStateAsync(string checkContext,
            DynamicLinqParameters preStateParameters,
            Func<dynamic, bool> isExpectedPreState) {
            var pre = await GetFromDynamicLinqAsync(preStateParameters);
            if (!isExpectedPreState(pre)) {
                var preJson = JsonSerializer.Serialize<dynamic>(pre);
                IEnumerable<KeyValuePair<string, object>> dict = new List<KeyValuePair<string, object>> {
                    KeyValuePair.Create("PreState",(object)preJson),
                };
                var ex = new ApplicationException($"{GetType().Name} pre-state check failed for {checkContext}");
                using (Logger.BeginScope(dict)) {
                    Logger.LogError(ex, "For user {User}, {Repo} pre-state check failed for {CheckContext}", ScopeProperties?.User ?? "?", GetType().Name, checkContext);
                }
                throw ex;
            }
            return pre;
        }


        public virtual void CheckChange(string checkContext, dynamic pre,
            DynamicLinqParameters postStateParameters,
            Func<dynamic, dynamic, bool> isExpectedChange) {

            var post = GetFromDynamicLinq(postStateParameters);

            if (!isExpectedChange(pre, post)) {
                var preJson = JsonSerializer.Serialize<dynamic>(pre);
                var postJson = JsonSerializer.Serialize<dynamic>(post);
                IEnumerable<KeyValuePair<string, object>> dict = new List<KeyValuePair<string, object>> {
                    KeyValuePair.Create("PreState",(object)preJson),
                    KeyValuePair.Create("PostState",(object)postJson),
                };
                var ex = new ApplicationException($"{GetType().Name} check failed for {checkContext}");
                using (Logger.BeginScope(dict)) {
                    Logger.LogError(ex, "For user {User}, {Repo} Check failed for {CheckContext}", ScopeProperties?.User ?? "?", GetType().Name, checkContext);
                }
                throw ex;
            }
        }


        public virtual async Task CheckChangeAsync(string checkContext, dynamic pre,
            DynamicLinqParameters postStateParameters,
            Func<dynamic, dynamic, bool> isExpectedChange) {

            var post = await GetFromDynamicLinqAsync(postStateParameters);

            if (!isExpectedChange(pre, post)) {
                var preJson = JsonSerializer.Serialize<dynamic>(pre);
                var postJson = JsonSerializer.Serialize<dynamic>(post);
                IEnumerable<KeyValuePair<string, object>> dict = new List<KeyValuePair<string, object>> {
                    KeyValuePair.Create("PreState",(object)preJson),
                    KeyValuePair.Create("PostState",(object)postJson),
                };
                var ex = new ApplicationException($"{GetType().Name} check failed for {checkContext}");
                using (Logger.BeginScope(dict)) {
                    Logger.LogError(ex, "For user {User}, {Repo} Check failed for {CheckContext}", ScopeProperties?.User ?? "?", GetType().Name, checkContext);
                }
                throw ex;
            }
        }


        public virtual TEntity Execute(Operation operation, dynamic entity, params object[] keyValues) {
            switch (operation) {
                case Operation.Create:
                    return Create(entity);
                case Operation.Update:
                    return UpdateAsync(entity, keyValues);
                case Operation.Delete:
                    Delete(keyValues);
                    return null;
                default:
                    return null;
            }
        }


        public virtual async Task<TEntity> ExecuteAsync(Operation operation, dynamic entity, params object[] keyValues) {
            switch (operation) {
                case Operation.Create:
                    return await CreateAsync(entity);
                case Operation.Update:
                    return await UpdateAsync(entity, keyValues);
                case Operation.Delete:
                    await DeleteAsync(keyValues);
                    return await Task.FromResult<TEntity>(null);
                default:
                    return await Task.FromResult<TEntity>(null);
            }
        }

    }


}

