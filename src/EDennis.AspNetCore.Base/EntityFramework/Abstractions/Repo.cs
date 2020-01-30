using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Serialization;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    [ScopedTraceLogger]
    [AspectSkipProperties(true)]
    public partial class Repo<TEntity, TContext> : IRepo, IRepo<TEntity, TContext>
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext {

        public TContext Context { get; set; }
        public IScopeProperties ScopeProperties { get; set; }


        public static PropertyInfo[] Properties;
        public static Func<string, object[]> ParseId;
        public static Func<dynamic, object[]> GetPrimaryKeyDynamic;
        public static Func<TEntity, object[]> GetPrimaryKey;
        public static IReadOnlyList<IProperty> KeyProperties;


        static Repo() {

            Properties = typeof(TEntity).GetProperties();

            ParseId = (s) => {
                var key = s.Split('~');
                var id = new object[KeyProperties.Count];
                try {
                    for (int i = 0; i < id.Length; i++)
                        id[i] = Convert.ChangeType(key[i], KeyProperties[i].ClrType);
                } catch {
                    throw new ArgumentException($"The provided path parameters ({key}) cannot be converted into a key for {typeof(TEntity).Name}");
                }
                return id;
            };

            GetPrimaryKeyDynamic = (dyn) => {
                var id = new object[KeyProperties.Count];
                Type dynType = dyn.GetType();
                PropertyInfo[] props = dynType.GetProperties();
                for (int i = 0; i < KeyProperties.Count; i++) {
                    var keyName = KeyProperties[i].Name;
                    var keyType = KeyProperties[i].ClrType;
                    var prop = props.FirstOrDefault(p => p.Name == keyName);
                    if (prop == null)
                        throw new ArgumentException($"The provided input does not contain a {keyName} property");
                    var keyValue = prop.GetValue(dyn);
                    id[i] = Convert.ChangeType(keyValue, keyType);
                }
                return id;
            };

            GetPrimaryKey = (entity) => {
                var id = new object[KeyProperties.Count];
                for (int i = 0; i < KeyProperties.Count; i++) {
                    var keyName = KeyProperties[i].Name;
                    var keyType = KeyProperties[i].ClrType;
                    var prop = Properties.FirstOrDefault(p => p.Name == keyName);
                    var keyValue = prop.GetValue(entity);
                    id[i] = Convert.ChangeType(keyValue, keyType);
                }
                return id;
            };
        }



        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public Repo(DbContextProvider<TContext> provider,
            IScopeProperties scopeProperties) {

            Context = provider.Context;
            ScopeProperties = scopeProperties;

            if (KeyProperties == null)
                KeyProperties = Context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties;

        }



        /// <summary>
        /// Retrieves the entity with the provided primary key values
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>
        public virtual TEntity Get(params object[] keyValues) {
            var entity = Context.Find<TEntity>(keyValues);
            if (entity != null)
                Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }


        /// <summary>
        /// Asychronously retrieves the entity with the provided primary key values.
        /// </summary>
        /// <param name="keyValues">primary key provided as key-value object array</param>
        /// <returns>Entity whose primary key matches the provided input</returns>

        public virtual async Task<TEntity> GetAsync(params object[] keyValues) {
            var entity = await Context.FindAsync<TEntity>(keyValues);
            if (entity != null)
                Context.Entry(entity).State = EntityState.Detached;
            return entity;
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

        public virtual DynamicLinqResult GetWithDynamicLinq(
                string select,
                string where = null,
                string orderBy = null,
                int? skip = null,
                int? take = null,
                int? totalRecords = null) {

            IQueryable qry = BuildLinqQuery(select, where, orderBy, skip, take, totalRecords,
                out DynamicLinqResult pagedResult);

            var result = qry.ToDynamicList();
            pagedResult.Data = result;

            return pagedResult;

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

        public virtual async Task<DynamicLinqResult> GetWithDynamicLinqAsync(
                string select,
                string where = null,
                string orderBy = null,
                int? skip = null,
                int? take = null,
                int? totalRecords = null) {

            IQueryable qry = BuildLinqQuery(select, where, orderBy, skip, take, totalRecords,
                out DynamicLinqResult pagedResult);

            var result = await qry.ToDynamicListAsync();
            pagedResult.Data = result;

            return pagedResult;

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

        public virtual DynamicLinqResult<TEntity> GetWithDynamicLinq(
                string where = null,
                string orderBy = null,
                int? skip = null,
                int? take = null,
                int? totalRecords = null) {

            IQueryable<TEntity> qry = BuildLinqQuery(where, orderBy, skip, take, totalRecords,
                out DynamicLinqResult<TEntity> pagedResult);

            var result = qry.ToDynamicList<TEntity>();
            pagedResult.Data = result;

            return pagedResult;

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

        public virtual async Task<DynamicLinqResult<TEntity>> GetWithDynamicLinqAsync(
                string where = null,
                string orderBy = null,
                int? skip = null,
                int? take = null,
                int? totalRecords = null) {

            IQueryable<TEntity> qry = BuildLinqQuery(where, orderBy, skip, take, totalRecords,
                out DynamicLinqResult<TEntity> pagedResult);

            var result = await qry.ToDynamicListAsync<TEntity>();
            pagedResult.Data = result;

            return pagedResult;
        }






        private IQueryable BuildLinqQuery(string select, string where, string orderBy, int? skip, int? take, int? totalRecords, out DynamicLinqResult pagedResult) {

            IQueryable qry = Query;

            try {
                if (!string.IsNullOrWhiteSpace(where))
                    qry = qry.Where(where);
                if (!string.IsNullOrWhiteSpace(orderBy))
                    qry = qry.OrderBy(orderBy);
                if (!string.IsNullOrWhiteSpace(select))
                    qry = qry.Select(select);
            } catch (ParseException ex) {
                throw new ArgumentException(ex.Message);
            }

            if (totalRecords == null || totalRecords.Value < 0)
                totalRecords = qry.Count();

            var skipValue = skip == null ? 0 : skip.Value;
            var takeValue = take == null ? totalRecords.Value - skipValue : take.Value;
            var pageCount = (int)Math.Ceiling(totalRecords.Value / (double)takeValue);

            pagedResult = new DynamicLinqResult {
                CurrentPage = 1 + (int)Math.Ceiling((skipValue) / (double)takeValue),
                PageCount = pageCount,
                PageSize = takeValue,
                RowCount = totalRecords.Value
            };
            if (skipValue != 0)
                qry = qry.Skip(skipValue);
            if (take != null && take.Value > 0)
                qry = qry.Take(takeValue);

            return qry;
        }



        private IQueryable<TEntity> BuildLinqQuery(string where, string orderBy, int? skip, int? take, int? totalRecords, out DynamicLinqResult<TEntity> pagedResult) {

            IQueryable<TEntity> qry = Query;

            try {
                if (!string.IsNullOrWhiteSpace(where))
                    qry = qry.Where(where);
                if (!string.IsNullOrWhiteSpace(orderBy))
                    qry = qry.OrderBy(orderBy);
            } catch (ParseException ex) {
                throw new ArgumentException(ex.Message);
            }

            if (totalRecords == null || totalRecords.Value < 0)
                totalRecords = qry.Count();

            var skipValue = skip == null ? 0 : skip.Value;
            var takeValue = take == null ? totalRecords.Value - skipValue : take.Value;
            var pageCount = (int)Math.Ceiling(totalRecords.Value / (double)takeValue);

            pagedResult = new DynamicLinqResult<TEntity> {
                CurrentPage = 1 + (int)Math.Ceiling((skipValue) / (double)takeValue),
                PageCount = pageCount,
                PageSize = takeValue,
                RowCount = totalRecords.Value
            };
            if (skipValue != 0)
                qry = qry.Skip(skipValue);
            if (take != null && take.Value > 0)
                qry = qry.Take(takeValue);

            return qry;
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
                throw new ArgumentException(
                    $"Cannot create a null {typeof(TEntity).Name}");

            SetSysUser(entity);
            try {
                Context.Add(entity);
                Context.SaveChanges();
            } catch (DbUpdateException ex) {
                throw new DbOperationException($"Cannot create {typeof(TEntity).Name}: {ex.InnerException.Message}", ex.InnerException);
            }
            return entity;
        }

        /// <summary>
        /// Asynchronously creates a new entity from the provided input
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity) {
            if (entity == null)
                throw new ArgumentException(
                    $"Cannot create a null {typeof(TEntity).Name}");

            SetSysUser(entity);

            try {
                Context.Add(entity);
                await Context.SaveChangesAsync();
            } catch (DbUpdateException ex) {
                throw new DbOperationException($"Cannot create {typeof(TEntity).Name}: {ex.InnerException.Message}", ex.InnerException);
            }
            return entity;
        }




        /// <summary>
        /// Updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual TEntity Update(TEntity entity, params object[] keyValues) {
            if (entity == null)
                throw new ArgumentException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            //retrieve the existing entity
            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"No {typeof(TEntity).Name} exists with {keyValues.ToTildaDelimited()}");

            SetSysUser(entity);

            //copy property values from entity to existing (resultEntity)
            Context.Entry(existing).CurrentValues.SetValues(entity);
            Context.Entry(existing).State = EntityState.Detached;

            try {
                Context.Update(entity);
                Context.SaveChanges();
            } catch (DbUpdateException ex) {
                throw new DbOperationException($"Cannot update {typeof(TEntity).Name}: {ex.InnerException.Message}", ex.InnerException);
            }
            return existing;
        }




        /// <summary>
        /// Asynchronously updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, params object[] keyValues) {

            if (entity == null)
                throw new ArgumentException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            //retrieve the existing entity (resultEntity)
            var existing = await Context.FindAsync<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"No {typeof(TEntity).Name} exists with {keyValues.ToTildaDelimited()}");

            SetSysUser(entity);

            //copy property values from entity to existing
            Context.Entry(existing).CurrentValues.SetValues(entity);
            Context.Entry(existing).State = EntityState.Detached;

            try {
                Context.Update(entity);
                await Context.SaveChangesAsync();
            } catch (DbUpdateException ex) {
                throw new DbOperationException($"Cannot update {typeof(TEntity).Name}: {ex.InnerException.Message}", ex.InnerException);
            }
            return existing;
        }


        /// <summary>
        /// Updates an entity from data provided by a partial entity
        /// </summary>
        /// <param name="partialEntity">data to update</param>
        /// <param name="keyValues">the primary key</param>
        /// <returns></returns>
        public virtual TEntity Patch(dynamic partialEntity, params object[] keyValues) {
            if (partialEntity == null)
                throw new ArgumentException(
                    $"Cannot update a null {typeof(TEntity).Name}");


            //retrieve the existing entity
            TEntity existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"No {typeof(TEntity).Name} exists with {keyValues.ToTildaDelimited()}");

            //copy property values from entity to existing
            try {
                Projection<TEntity>.Patch(partialEntity, existing);
            } catch {
                throw new ArgumentException($"Cannot patch/update {existing.GetType().Name} with '{JsonSerializer.Serialize(partialEntity)}'");
            }

            existing.SysUser = ScopeProperties.User;

            Context.Entry(existing).State = EntityState.Detached;

            try {
                Context.Update(existing);
                Context.SaveChanges();
            } catch (DbUpdateException ex) {
                throw new DbOperationException($"Cannot patch/update {typeof(TEntity).Name}: {ex.InnerException.Message}", ex.InnerException);
            }
            return existing; //updated entity

        }


        /// <summary>
        /// Asynchronously updates an entity from data provided by a partial entity
        /// </summary>
        /// <param name="partialEntity">data to update</param>
        /// <param name="keyValues">the primary key</param>
        /// <returns></returns>
        public virtual async Task<TEntity> PatchAsync(dynamic partialEntity, params object[] keyValues) {
            if (partialEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            //retrieve the existing entity
            var existing = await Context.FindAsync<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"No {typeof(TEntity).Name} exists with {keyValues.ToTildaDelimited()}");

            //copy property values from entity to existing
            try {
                Projection<TEntity>.Patch(partialEntity, existing);
            } catch {
                throw new ArgumentException($"Cannot patch/update {existing.GetType().Name} with '{JsonSerializer.Serialize(partialEntity)}'");
            }
            existing.SysUser = ScopeProperties.User;
            Context.Entry(existing).State = EntityState.Detached;

            try {
                Context.Update(existing);
                await Context.SaveChangesAsync();
            } catch (DbUpdateException ex) {
                throw new DbOperationException($"Cannot update/patch {typeof(TEntity).Name}: {ex.InnerException.Message}", ex.InnerException);
            }
            return existing; //updated entity
        }




        /// <summary>
        /// Deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual void Delete(params object[] keyValues) {

            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                return;
                //throw new MissingEntityException(
                //    $"Cannot find {typeof(TEntity).Name} object with key value = {PrintKeys(keyValues)}");

            try {
                Context.Remove(existing);
                Context.SaveChanges();
            } catch (DbUpdateConcurrencyException ex) {
                Context.Entry(existing).State = EntityState.Detached;
                if (Context.Find<TEntity>(existing) != null)
                    throw new DbOperationException(ex.InnerException.Message, ex.InnerException);
            }

        }


        /// <summary>
        /// Asynchrously deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public virtual async Task DeleteAsync(params object[] keyValues) {
            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                return;
            //    throw new MissingEntityException(
            //        $"Cannot find {typeof(TEntity).Name} object with key value = {PrintKeys(keyValues)}");

            try {
                Context.Remove(existing);
                await Context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException ex) {
                Context.Entry(existing).State = EntityState.Detached;
                if (Context.Find<TEntity>(existing) != null)
                    throw new DbOperationException(ex.InnerException.Message, ex.InnerException);
            }
        }


        #region helper methods
        protected void SetSysUser(dynamic entity) { entity.SysUser = ScopeProperties.User; }
        protected void SetSysUser(TEntity entity) { entity.SysUser = ScopeProperties.User; }
        protected static string PrintKeys(params object[] keyValues) {
            return "[" + string.Join(",", keyValues) + "]";
        }
        #endregion


    }


}

