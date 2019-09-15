using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public abstract class WriteableRepo<TEntity, TContext> : IRepo
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext {


        public TContext Context { get; set; }
        public ScopeProperties ScopeProperties { get; set; }
        private readonly ILogger _logger;
        private readonly string _className;

        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public WriteableRepo(TContext context, ScopeProperties scopeProperties,
            ILogger<WriteableRepo<TEntity, TContext>> logger = null) {
            Context = context;
            ScopeProperties = scopeProperties;

            if (logger == null) {
                _logger = new LoggerFactory().CreateLogger(GetType().Name);
            }

            _className = $"{GetType().Name} (WriteableRepo)";
            var expandedClassName = $"{GetType().Name} (WriteableRepo<{typeof(TEntity).Name},{typeof(TContext).Name}>)";

            _logger.LogConstructor(expandedClassName, ScopeProperties.User);
        }



        /// <summary>
        /// Retrieves the entity with the provided primary key values
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>
        public virtual TEntity GetById(params object[] keyValues) {
            _logger.LogMethodEnter($"{_className}.GetById", ScopeProperties.User,
                new { keyValues, ScopeProperties });

            TEntity result;
            try {
                _logger.LogStatement($"{_className}.GetById", "Context.Find<TEntity>(keyValues)", ScopeProperties.User);
                result = Context.Find<TEntity>(keyValues);
            } catch (Exception ex) {
                _logger.LogException($"{_className}.GetById", ScopeProperties.User, ex, 
                    new { keyValues, ScopeProperties});
                throw;
            }
            _logger.LogMethodExit($"{_className}.GetById", ScopeProperties.User,
                new { ReturnValue = result });
            return result;
        }


        /// <summary>
        /// Asychronously retrieves the entity with the provided primary key values.
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>
        public virtual async Task<TEntity> GetByIdAsync(params object[] keyValues) {
            _logger.LogMethodEnter($"{_className}.GetByIdAsync", ScopeProperties.User,
                new { keyValues, ScopeProperties });

            TEntity result;

            try {
                _logger.LogStatement($"{_className}.GetByIdAsync", "Context.FindAsync<TEntity>(keyValues)", 
                    ScopeProperties.User, new { keyValues, ScopeProperties });
                result = await Context.FindAsync<TEntity>(keyValues);
            } catch (Exception ex) {
                _logger.LogException($"{_className}.GetByIdAsync", ScopeProperties.User, 
                    ex, new { keyValues, ScopeProperties });
                throw;
            }

            _logger.LogMethodExit($"{_className}.GetByIdAsync", ScopeProperties.User,
                new { Arguments = keyValues, ScopeProperties, ReturnValue = result });

            return result;
        }


        public IQueryable<TEntity> Query {
            get {
                _logger.LogStatement($"{_className}.Query.get", "Context.Set<TEntity>()...",
                    ScopeProperties.User, new { ScopeProperties });
                return Context.Set<TEntity>()
                    .AsNoTracking();
            }
        }



        /// <summary>
        /// Determines if an object with the given primary key values
        /// exists in the context.
        /// </summary>
        /// <param name="keyValues">primary key values</param>
        /// <returns>true if an entity with the provided keys exists</returns>
        public bool Exists(params object[] keyValues) {
            _logger.LogMethodEnter($"{_className}.Exists", ScopeProperties.User,
                new { keyValues, ScopeProperties });

            var entity = Context.Find<TEntity>(keyValues);
            if (entity != null)
                Context.Entry(entity).State = EntityState.Detached;

            var returnValue = (entity != null);

            _logger.LogMethodExit($"{_className}.Exists", ScopeProperties.User,
                new { keyValues, ScopeProperties, returnValue });

            return returnValue;
        }


        /// <summary>
        /// Determines if an object with the given primary key values
        /// exists in the context.
        /// </summary>
        /// <param name="keyValues">primary key values</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(params object[] keyValues) {
            _logger.LogMethodEnter($"{_className}.ExistsAsync", ScopeProperties.User,
                new { keyValues, ScopeProperties });

            var entity = await Context.FindAsync<TEntity>(keyValues);
            if (entity != null)
                Context.Entry(entity).State = EntityState.Detached;

            var returnValue = (entity != null);

            _logger.LogMethodExit($"{_className}.ExistsAsync", ScopeProperties.User,
                new { keyValues, ScopeProperties, returnValue });

            return returnValue;
        }


        /// <summary>
        /// Creates a new entity from the provided input
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity</returns>
        public virtual TEntity Create(TEntity entity) {
            _logger.LogMethodEnter($"{_className}.Create", ScopeProperties.User,
                new { entity, ScopeProperties });

            if (entity == null) {
                throw new MissingEntityException(
                    "Null Entity Provided to Create Method",
                    $"Cannot create a null {entity.GetType().Name}");
            }
            if (entity.SysUser == null)
                entity.SysUser = ScopeProperties.User;

            Context.Add(entity);
            Context.SaveChanges();

            _logger.LogMethodExit($"{_className}.Create", ScopeProperties.User,
                new { entity, ScopeProperties, returnValue = entity });

            return entity;
        }


        /// <summary>
        /// Asynchronously creates a new entity from the provided input
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity) {
            _logger.LogMethodEnter($"{_className}.CreateAsync", ScopeProperties.User,
                new { entity, ScopeProperties });

            if (entity == null)
                throw new MissingEntityException(
                    "Null Entity Provided to Create Method",
                    $"Cannot create a null {entity.GetType().Name}");

            if (entity.SysUser == null)
                entity.SysUser = ScopeProperties.User;

            Context.Add(entity);
            await Context.SaveChangesAsync();

            _logger.LogMethodExit($"{_className}.CreateAsync", ScopeProperties.User,
                new { entity, ScopeProperties, returnValue = entity });

            return entity;
        }


        /// <summary>
        /// Updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual TEntity Update(TEntity entity, params object[] keyValues) {
            _logger.LogMethodEnter($"{_className}.Update", ScopeProperties.User,
                new { entity, keyValues, ScopeProperties });

            if (entity == null)
                throw new MissingEntityException(
                    "Null Entity Provided to Update Method",
                    $"Cannot update a null {entity.GetType().Name}");

            //retrieve the existing entity
            var existing = Context.Find<TEntity>(keyValues);

            if (existing == null)
                throw new MissingEntityException(
                    "Entity To Update Cannot Be Found",
                    $"Cannot find {entity.GetType().Name} with key = {PrintKeys()}");

            if (entity.SysUser == null)
                entity.SysUser = ScopeProperties.User;

            //copy property values from entity to existing
            Context.Entry(existing).CurrentValues.SetValues(entity);

            Context.Update(existing); 
            Context.SaveChanges();

            _logger.LogMethodExit($"{_className}.Update", ScopeProperties.User,
                new { entity, keyValues, ScopeProperties, returnValue = existing });

            return existing;
        }


        public virtual TEntity Update(dynamic partialEntity, params object[] keyValues) {
            _logger.LogMethodEnter($"{_className}.Update", ScopeProperties.User,
                new { partialEntity, keyValues, ScopeProperties });
            if (partialEntity == null)
                throw new MissingEntityException(
                    "Null Entity Provided to Update Method",
                    $"Cannot update a null {typeof(TEntity).Name}");

            List<string> props = DynamicExtensions.GetProperties(partialEntity);
            if (!props.Contains("SysUser") || partialEntity.SysUser == null)
                partialEntity.SysUser = ScopeProperties.User;

            //retrieve the existing entity
            var existing = Context.Find<TEntity>(keyValues);

            if (existing == null) {
                throw new MissingEntityException(
                    "Entity To Update Cannot Be Found",
                    $"Cannot find {typeof(TEntity).Name} with key = {PrintKeys()}");
            }

            //TODO: statement log

            //copy property values from entity to existing
            DynamicExtensions.Populate<TEntity>(existing, partialEntity);

            Context.Update(existing);
            Context.SaveChanges();
            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogMethodExit($"{_className}.Update", ScopeProperties.User,
                new { partialEntity, keyValues, ScopeProperties, returnValue = existing });
#endif
            #endregion
            return existing; //updated entity
        }


        /// <summary>
        /// Asynchronously updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, params object[] keyValues) {
            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogMethodEnter($"{_className}.UpdateAsync", ScopeProperties.User,
                new { entity, keyValues, ScopeProperties });
#endif
            #endregion

            if (entity == null) {
                #region MissingEntityException
                LogAndThrowMissingEntityException($"{_className}.UpdateAsync",
                    "Null Entity Provided to Update Method",
                    $"Cannot update a null {entity.GetType().Name}");
                #endregion
            }

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogStatement($"{_className}.UpdateAsync", "var existing = await Context.FindAsync<TEntity>(keyValues);",
                ScopeProperties.User, new { entity, keyValues, ScopeProperties });
#endif
            #endregion
            //retrieve the existing entity
            var existing = await Context.FindAsync<TEntity>(keyValues);

            if (existing == null) {
                #region MissingEntityException
                LogAndThrowMissingEntityException($"{_className}.UpdateAsync",
                    "Entity To Update Cannot Be Found",
                    $"Cannot find {entity.GetType().Name} with key = {PrintKeys()}");
                #endregion
            }
            if (entity.SysUser == null)
                entity.SysUser = ScopeProperties.User;

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogStatement($"{_className}.UpdateAsync", "Context.Entry(existing).CurrentValues.SetValues(entity);",
                ScopeProperties.User, new { existing, entity, keyValues, ScopeProperties });
#endif
            #endregion
            //copy property values from entity to existing
            Context.Entry(existing).CurrentValues.SetValues(entity);

            try {
                #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
                _logger.LogStatement($"{_className}.UpdateAsync", 
                    "Context.Update(existing);\nawait Context.SaveChangesAsync();",
                    ScopeProperties.User,
                    new { entity, keyValues, ScopeProperties, returnValue = existing });
#endif
                #endregion
                Context.Update(existing);
                await Context.SaveChangesAsync();

            } catch (Exception ex) {
                _logger.LogException($"{_className}.UpdateAsync", ScopeProperties.User,
                    ex, new { entity, keyValues, ScopeProperties, returnValue = existing });
                throw;
            }

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogMethodExit($"{_className}.Update", ScopeProperties.User,
                new { entity, keyValues, ScopeProperties, returnValue = existing });
#endif
            #endregion
            return existing;
        }


        public virtual async Task<TEntity> UpdateAsync(dynamic partialEntity, params object[] keyValues) {
            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogMethodEnter($"{_className}.UpdateAsync", ScopeProperties.User,
                new { partialEntity, keyValues, ScopeProperties });
#endif
            #endregion
            if (partialEntity == null) {
                #region MissingEntityException
                LogAndThrowMissingEntityException($"{_className}.UpdateAsync",
                    "Null Entity Provided to Update Method",
                    $"Cannot update a null {typeof(TEntity).Name}"
                    );
                #endregion
            }

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogStatement($"{_className}.UpdateAsync", "List<string> props = DynamicExtensions.GetProperties(partialEntity);",
                ScopeProperties.User, new { partialEntity, keyValues, ScopeProperties });
#endif
            #endregion
            //retrieve the existing entity
            List<string> props = DynamicExtensions.GetProperties(partialEntity);

            if (!props.Contains("SysUser") || partialEntity.SysUser == null)
                partialEntity.SysUser = ScopeProperties.User;

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogStatement($"{_className}.UpdateAsync", "var existing = await Context.FindAsync<TEntity>(keyValues);",
                ScopeProperties.User, new { partialEntity, keyValues, ScopeProperties });
#endif
            #endregion
            //retrieve the existing entity
            var existing = await Context.FindAsync<TEntity>(keyValues);

            if (existing == null) {
                #region MissingEntityException
                LogAndThrowMissingEntityException($"{_className}.UpdateAsync",
                    "Entity To Delete Cannot Be Found",
                    $"Cannot find {new TEntity().GetType().Name} object with key value = {PrintKeys(keyValues)}"
                    );
                #endregion
            }

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogStatement($"{_className}.UpdateAsync", "DynamicExtensions.Populate<TEntity>(existing, partialEntity);",
                ScopeProperties.User, new { existing, partialEntity, keyValues, ScopeProperties });
#endif
            #endregion
            //copy property values from entity to existing
            DynamicExtensions.Populate<TEntity>(existing, partialEntity);

            try {
                #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
                _logger.LogStatement($"{_className}.UpdateAsync", "Context.Update(existing);\nawait Context.SaveChangesAsync();",
                    ScopeProperties.User, new { partialEntity, keyValues, ScopeProperties });
#endif
                #endregion
                Context.Update(existing);
                await Context.SaveChangesAsync();
            } catch (Exception ex) {
                _logger.LogException($"{_className}.UpdateAsync", ScopeProperties.User,
                    ex, new { partialEntity, keyValues, ScopeProperties, existing });
            }

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogMethodExit($"{_className}.UpdateAsync", ScopeProperties.User,
                new { partialEntity, keyValues, ScopeProperties, returnValue = existing });
#endif
            #endregion
            return existing; //updated entity
        }


        /// <summary>
        /// Deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual void Delete(params object[] keyValues) {
            _logger.LogMethodEnter($"{_className}.Delete", ScopeProperties.User,
                new { keyValues, ScopeProperties });

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogStatement($"{_className}.Delete", "var existing = Context.Find<TEntity>(keyValues);",
                ScopeProperties.User, new { keyValues, ScopeProperties });
#endif
            #endregion
            var existing = Context.Find<TEntity>(keyValues);

            if (existing == null) {
                #region MissingEntityException
                LogAndThrowMissingEntityException($"{_className}.Delete",
                    "Entity To Delete Cannot Be Found",
                    $"Cannot find {new TEntity().GetType().Name} object with key value = {PrintKeys(keyValues)}"
                    );
                #endregion
            }

            try {
                #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
                _logger.LogStatement($"{_className}.Delete", "Context.Remove(existing);\nContext.SaveChanges();",
                    ScopeProperties.User, new { keyValues, ScopeProperties });
#endif
                #endregion
                Context.Remove(existing);
                Context.SaveChanges();
            } catch (Exception ex) {
                _logger.LogException($"{_className}.Delete", ScopeProperties.User,
                    ex, new { keyValues, ScopeProperties });
                throw;
            }

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogMethodExit($"{_className}.Delete", ScopeProperties.User,
                new { keyValues, ScopeProperties });
#endif
            #endregion
        }

        /// <summary>
        /// Asynchrously deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual async Task DeleteAsync(params object[] keyValues) {
            _logger.LogMethodEnter($"{_className}.DeleteAsync", ScopeProperties.User,
                new { keyValues, ScopeProperties });

            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null) {
                #region MissingEntityException
                LogAndThrowMissingEntityException($"{_className}.DeleteAsync",
                    "Entity To Delete Cannot Be Found",
                    $"Cannot find {new TEntity().GetType().Name} object with key value = {PrintKeys(keyValues)}"
                    );
                #endregion
            }

            try {
                #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
                _logger.LogStatement($"{_className}.DeleteAsync", "Context.Remove(existing);\nawait Context.SaveChangesAsync();",
                    ScopeProperties.User, new { keyValues, ScopeProperties });
#endif
                #endregion
                Context.Remove(existing);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex) {
                _logger.LogException($"{_className}.DeleteAsync", ScopeProperties.User,
                    ex, new { keyValues, ScopeProperties });
                throw;
            }
            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogMethodExit($"{_className}.DeleteAsync", ScopeProperties.User,
                new { keyValues, ScopeProperties });
#endif
            #endregion
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
        public List<dynamic> GetFromDynamicLinq(
                string where = null,
                string orderBy = null,
                string select = null,
                int? skip = null,
                int? take = null) {

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogMethodEnter($"{_className}.GetFromDynamicLinq", ScopeProperties.User,
                new { where, orderBy, select, skip, take, ScopeProperties });
#endif
            #endregion

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

            try {
                #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
                _logger.LogStatement($"{_className}.GetFromDynamicLinq", "qry.ToDynamicList()",
                    ScopeProperties.User, new { where, orderBy, select, skip, take, ScopeProperties });
#endif
                #endregion
                var returnValue = qry.ToDynamicList();
                #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
                _logger.LogMethodExit($"{_className}.GetFromDynamicLinq", ScopeProperties.User,
                    new { where, orderBy, select, skip, take, ScopeProperties, returnValue });
#endif
                #endregion
                return returnValue;
            } catch (Exception ex) {
                _logger.LogException($"{_className}.GetFromDynamicLinq", ScopeProperties.User,
                    ex, new { where, orderBy, select, skip, take, ScopeProperties });
                throw;
            }

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

            #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
            _logger.LogMethodEnter($"{_className}.GetFromDynamicLinqAsync", ScopeProperties.User,
                new { where, orderBy, select, skip, take, ScopeProperties });
#endif
            #endregion
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

            try {
                #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
                _logger.LogStatement($"{_className}.GetFromDynamicLinqAsync", "await qry.ToDynamicListAsync()",
                    ScopeProperties.User, new { where, orderBy, select, skip, take, ScopeProperties});
#endif
                #endregion
                var returnValue = await qry.ToDynamicListAsync();
                #region tracelog
#if TRACE || TRACE_REPO //omit trace log call, unless TRACE or TRACE_REPO is defined
                _logger.LogMethodExit($"{_className}.GetFromDynamicLinqAsync", ScopeProperties.User,
                    new { where, orderBy, select, skip, take, ScopeProperties, returnValue });
#endif
                #endregion
                return returnValue;
            } catch (Exception ex) {
                _logger.LogException($"{_className}.GetFromDynamicLinqAsync", ScopeProperties.User,
                    ex, new { where, orderBy, select, skip, take, ScopeProperties });
                throw;
            }

        }


        protected string PrintKeys(params object[] keyValues) {
            return "[" + string.Join(",", keyValues) + "]";
        }


        protected void LogAndThrowMissingEntityException(string methodName, string title, string message) {

            _logger.LogWarning("MissingEntityException: Method: {Method}, User: {User}, ExceptionTitle: {Title}, ExceptionMessage: {Message}",
                methodName, ScopeProperties.User, title, message);

            throw new MissingEntityException(title,message);

        }



    }


}

