using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;

namespace EDennis.AspNetCore.Base.EntityFramework {

    [ScopedTraceLogger(logEntry:true)]
    [AspectSkipProperties(true)]
    public class TemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext> : Repo<TEntity, TContext>, ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext> 
        where TEntity : class, IEFCoreTemporalModel, new()
        where THistoryEntity : class, TEntity, new()
        where TContext : DbContext
        where THistoryContext : DbContext {

        public THistoryContext HistoryContext { get; set; }

        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public TemporalRepo(DbContextProvider<TContext> provider, DbContextProvider<THistoryContext> historyProvider,
            IScopeProperties scopeProperties,
            ILogger<Repo<TEntity, TContext>> logger) 
            : base(provider, scopeProperties, logger) {

            HistoryContext = historyProvider.Context;
            ScopeProperties = scopeProperties;
        }


        public virtual bool WriteUpdate(TEntity next, TEntity current)
            => true;
        //=> (DateTime.Now > current.SysStart.AddHours(8)
        //    || SysUser != current.SysUser);


        public virtual bool WriteUpdate(dynamic next, TEntity current)
            => true;
        //=> (DateTime.Now > current.SysStart.AddHours(8)
        //    || SysUser != current.SysUser);


        public virtual bool WriteDelete(TEntity current)
            => true;
        //=> (DateTime.Now > current.SysStart.AddHours(8)
        //    || SysUser != current.SysUser);



        /// <summary>
        /// Creates a new entity from the provided input
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity</returns>
        public new virtual TEntity Create(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot create a null {entity.GetType().Name}");

            SetSysProps(entity);

            Context.Add(entity);
            Context.SaveChanges();
            return entity;

        }


        /// <summary>
        /// Asynchronously creates a new entity from the provided input
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity</returns>
        public new virtual async Task<TEntity> CreateAsync(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot create a null {entity.GetType().Name}");

            SetSysProps(entity);

            Context.Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }


        /// <summary>
        /// Updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public new virtual TEntity Update(TEntity entity, params object[] keyValues) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            var existing = Context.Find<TEntity>(keyValues);

            SetSysProps(entity);


            if (WriteUpdate(entity, existing))
                WriteToHistory(existing);

            //copy property values from entity to existing
            Context.Entry(existing).CurrentValues.SetValues(entity);

            Context.Update(existing);
            Context.SaveChanges();
            return existing;

        }

        /// <summary>
        /// Asynchronously updates the provided entity
        /// </summary>
        /// <param name="entity">The new data for the entity</param>
        /// <returns>The newly updated entity</returns>
        public new virtual async Task<TEntity> UpdateAsync(TEntity entity, params object[] keyValues) {

            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            var existing = await Context.FindAsync<TEntity>(keyValues);

            SetSysProps(entity);

            if (WriteUpdate(entity, existing))
                await WriteToHistoryAsync(existing);

            Context.Entry(existing).CurrentValues.SetValues(entity);

            Context.Update(entity);
            await Context.SaveChangesAsync();
            return existing;
        }



        public new virtual TEntity Update(dynamic partialEntity, params object[] keyValues) {
            if (partialEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            SetSysProps(partialEntity);

            //retrieve the existing entity
            var existing = Context.Find<TEntity>(keyValues);

            if (WriteUpdate(partialEntity, existing))
                WriteToHistory(existing);

            //copy property values from entity to existing
            existing = ObjectUtils.CopyFromDynamic<TEntity>(partialEntity);
            //DynamicExtensions.Populate<TEntity>(existing, partialEntity);

            Context.Update(existing);
            Context.SaveChanges();
            return existing; //updated entity

        }


        public new virtual async Task<TEntity> UpdateAsync(dynamic partialEntity, params object[] keyValues) {

            if (partialEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            SetSysProps(partialEntity);

            //retrieve the existing entity
            var existing = await Context.FindAsync<TEntity>(keyValues);

            if (WriteUpdate(partialEntity, existing))
                await WriteToHistoryAsync(existing);

            //copy property values from entity to existing
            existing = ObjectUtils.CopyFromDynamic<TEntity>(partialEntity);
            //DynamicExtensions.Populate<TEntity>(existing, partialEntity);

            Context.Update(existing);
            await Context.SaveChangesAsync();
            return existing; //updated entity

        }

        /// <summary>
        /// Deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public new virtual void Delete(params object[] keyValues) {

            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {typeof(TEntity).Name} object with key value = {PrintKeys(keyValues)}");

            if (WriteDelete(existing))
                WriteToHistory(existing);

            Context.Remove(existing);
            Context.SaveChanges();

        }

        /// <summary>
        /// Asynchrously deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public new virtual async Task DeleteAsync(params object[] keyValues) {
            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {typeof(TEntity).Name} object with key value = {PrintKeys(keyValues)}");

            if (WriteDelete(existing))
                await WriteToHistoryAsync(existing);

            Context.Remove(existing);
            await Context.SaveChangesAsync();
            return;
        }



        public List<TEntity> QueryAsOf(DateTime from,
                DateTime to, Expression<Func<TEntity, bool>> predicate,
                int pageNumber = 1, int pageSize = 10000,
                params Expression<Func<TEntity, dynamic>>[] orderSelectors
                ) {

            var asOfPredicate = GetAsOfRangePredicate(from, to);

            var current = Context.Set<TEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            var history = HistoryContext.Set<THistoryEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            if (orderSelectors.Length > 0) {
                var currentOrdered = OrderByUtils
                    .BuildOrderedQueryable(current, orderSelectors)
                    .ToList();
                var historyOrdered = OrderByUtils
                    .BuildOrderedQueryable(history, orderSelectors)
                    .ToList();
                var orderedUnion = currentOrdered.Union(historyOrdered).ToList();
                return orderedUnion;
            }


            var union = current.ToList().Union(history.ToList()).ToList();

            return union;
        }


        public List<TEntity> QueryAsOf(DateTime asOf,
                Expression<Func<TEntity, bool>> predicate,
                int pageNumber = 1, int pageSize = 10000,
                params Expression<Func<TEntity, dynamic>>[] orderSelectors
                ) {

            var asOfPredicate = GetAsOfBetweenPredicate(asOf);

            var current = Context.Set<TEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            var history = HistoryContext.Set<THistoryEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            if (orderSelectors.Length > 0) {
                var currentOrdered = OrderByUtils
                    .BuildOrderedQueryable(current, orderSelectors)
                    .ToList();
                var historyOrdered = OrderByUtils
                    .BuildOrderedQueryable(history, orderSelectors)
                    .ToList();
                var orderedUnion = currentOrdered.Union(historyOrdered).ToList();
                return orderedUnion;
            }


            var union = current.ToList().Union(history.ToList()).ToList();

            return union;
        }


        public List<TEntity> GetWithIdHistory(params object[] key) {

            var primaryKeyPredicate = GetPrimaryKeyPredicate(key);

            var current = Context.Set<TEntity>()
                .Where(primaryKeyPredicate)
                .AsNoTracking()
                .ToList();

            var history = HistoryContext.Set<THistoryEntity>()
                .Where(primaryKeyPredicate)
                .AsNoTracking()
                .ToList();

            var all = current.Union(history).ToList();
            return all;
        }



        public TEntity GetWithIdAsOf(DateTime asOf, params object[] key) {
            var primaryKeyPredicate = GetPrimaryKeyPredicate(key);
            var asOfPredicate = GetAsOfBetweenPredicate(asOf);

            var curr = Context.Set<TEntity>()
                .Where(primaryKeyPredicate)
                .Where(asOfPredicate)
                .AsNoTracking()
                .ToList();

            var history = HistoryContext.Set<THistoryEntity>()
                .Where(primaryKeyPredicate)
                .Where(asOfPredicate)
                .AsNoTracking()
                .ToList();

            var all = curr.Union(history).FirstOrDefault();
            return all;
        }


        private Expression<Func<TEntity, bool>> GetPrimaryKeyPredicate(params object[] key) {

            var keyProps = Context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties;

            var pe = Expression.Parameter(typeof(TEntity), "e");
            Expression finalExpression = null;

            for (int i = 0; i < keyProps.Count(); i++) {
                var pkProperty = keyProps[i];
                var type = typeof(TEntity);
                var left = Expression.Property(pe, type.GetProperty(pkProperty.Name));
                var right = Expression.Constant(key[i]);
                var eq = Expression.Equal(left, right);
                if (finalExpression != null)
                    finalExpression = Expression.AndAlso(finalExpression, eq);
                else
                    finalExpression = eq;
            }

            var expr = Expression.Lambda<Func<TEntity, bool>>(finalExpression, pe);

            return expr;
        }



        public virtual Expression<Func<TEntity, bool>> GetAsOfBetweenPredicate(DateTime asOf) {

            var type = typeof(TEntity);

            var pe = Expression.Parameter(type, "e");

            var sysStartProp = type.GetProperty("SysStart");
            var sysEndProp = type.GetProperty("SysEnd");

            var left1 = Expression.Property(pe, sysStartProp);
            var right1 = Expression.Constant(asOf);
            var ge = Expression.LessThanOrEqual(left1, right1);

            var left2 = Expression.Property(pe, sysEndProp);
            var right2 = Expression.Constant(asOf);
            var le = Expression.GreaterThanOrEqual(left2, right2);


            var between = Expression.AndAlso(ge, le);

            var expr = Expression.Lambda<Func<TEntity, bool>>(between, pe);

            return expr;
        }


        public virtual Expression<Func<TEntity, bool>> GetAsOfRangePredicate(DateTime from, DateTime to) {

            var type = typeof(TEntity);

            var pe = Expression.Parameter(type, "e");

            var sysStartProp = type.GetProperty("SysStart");
            var sysEndProp = type.GetProperty("SysEnd");

            var left1 = Expression.Property(pe, sysStartProp);
            var right1 = Expression.Constant(to);
            var ge = Expression.LessThanOrEqual(left1, right1);

            var left2 = Expression.Property(pe, sysEndProp);
            var right2 = Expression.Constant(from);
            var le = Expression.GreaterThanOrEqual(left2, right2);

            var between = Expression.AndAlso(ge, le);

            var expr = Expression.Lambda<Func<TEntity, bool>>(between, pe);

            return expr;
        }



        public virtual void WriteToHistory(TEntity existing) {
            var historyEntity = ObjectUtils.CopyFromBaseClass<TEntity, THistoryEntity>(existing);
            historyEntity.SysEnd = DateTime.Now.AddTicks(-1);
            historyEntity.SysUserNext = ScopeProperties.User;
            HistoryContext.Add(historyEntity);
            HistoryContext.SaveChanges();
        }


        public virtual async Task WriteToHistoryAsync(TEntity existing) {
            var historyEntity = ObjectUtils.CopyFromBaseClass<TEntity, THistoryEntity>(existing);
            historyEntity.SysEnd = DateTime.Now.AddTicks(-1);
            historyEntity.SysUserNext = ScopeProperties.User;
            HistoryContext.Add(historyEntity);
            await HistoryContext.SaveChangesAsync();
        }




        #region helper methods
        protected void SetSysProps(dynamic entity) { SetSysUser(entity); SetSysStart(entity); SetSysEnd(entity); }
        protected void SetSysProps(TEntity entity) { SetSysUser(entity); SetSysStart(entity); SetSysEnd(entity); }
        protected void SetSysUserNext(TEntity entity) { entity.SysUserNext = entity.SysUser; }
        protected void SetSysUserNext(dynamic entity) { entity.SysUserNext = entity.SysUser; }
        protected void SetSysStart(TEntity entity) { entity.SysStart = DateTime.Now; }
        protected void SetSysStart(dynamic entity) { entity.SysStart = DateTime.Now; }
        protected void SetSysEnd(TEntity entity) { entity.SysEnd = DateTime.MaxValue; }
        protected void SetSysEnd(dynamic entity) { entity.SysEnd = DateTime.MaxValue; }
        #endregion region



    }
}
