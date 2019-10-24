using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class TemporalRepo<TEntity, TContext, THistoryContext> : Repo<TEntity,TContext>,
        ITemporalRepo<TEntity, TContext, THistoryContext> 
        where TEntity : class, new()
        where TContext : DbContext
        where THistoryContext : DbContext {

        public THistoryContext HistoryContext { get; set; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public TemporalRepo(TContext context, THistoryContext historyContext,
            ScopeProperties scopeProperties,
            ILogger<Repo<TEntity, TContext>> logger) : base(context, scopeProperties, logger) {

            Context = context;
            HistoryContext = historyContext;
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

            SetSpecialPropertyValue(entity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValue(entity, "SysStart", DateTime.Now, true, true);
            SetSpecialPropertyValue(entity, "SysEnd", DateTime.MaxValue, true, true);
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
        public new virtual async Task<TEntity> CreateAsync(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot create a null {entity.GetType().Name}");

            SetSpecialPropertyValue(entity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValue(entity, "SysStart", DateTime.Now, true, true);
            SetSpecialPropertyValue(entity, "SysEnd", DateTime.MaxValue, true, true);
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
        public new virtual TEntity Update(TEntity entity, params object[] keyValues){
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            var existing = Context.Find<TEntity>(keyValues);

            SetSpecialPropertyValue(entity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValue(entity, "SysStart", DateTime.Now, true, true);
            SetSpecialPropertyValue(entity, "SysEnd", DateTime.MaxValue, true, true);

            if (WriteUpdate(entity, existing))
                WriteToHistory(existing);

            //copy property values from entity to existing
            Context.Entry(existing).CurrentValues.SetValues(entity);

            if (BeforeUpdate(entity, keyValues)) {
                Context.Update(existing);
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
        public new virtual async Task<TEntity> UpdateAsync(TEntity entity, params object[] keyValues) {

            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            var existing = await Context.FindAsync<TEntity>(keyValues);

            SetSpecialPropertyValue(entity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValue(entity, "SysStart", DateTime.Now, true, true);
            SetSpecialPropertyValue(entity, "SysEnd", DateTime.MaxValue, true, true);

            if (WriteUpdate(entity, existing))
                await WriteToHistoryAsync(existing);

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



        public new virtual TEntity Update(dynamic partialEntity, params object[] keyValues){
            if (partialEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            SetSpecialPropertyValueDynamic(partialEntity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValueDynamic(partialEntity, "SysStart", DateTime.Now, true, true);
            SetSpecialPropertyValueDynamic(partialEntity, "SysEnd", DateTime.MaxValue, true, true);

            //retrieve the existing entity
            var existing = Context.Find<TEntity>(keyValues);

            if (WriteUpdate(partialEntity, existing))
                WriteToHistory(existing);

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


        public new virtual async Task<TEntity> UpdateAsync(dynamic partialEntity, params object[] keyValues){

            if (partialEntity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {typeof(TEntity).Name}");

            SetSpecialPropertyValueDynamic(partialEntity, "SysUser", ScopeProperties?.User, true, true);
            SetSpecialPropertyValueDynamic(partialEntity, "SysStart", DateTime.Now, true, true);
            SetSpecialPropertyValueDynamic(partialEntity, "SysEnd", DateTime.MaxValue, true, true);

            //retrieve the existing entity
            var existing = await Context.FindAsync<TEntity>(keyValues);

            if (WriteUpdate(partialEntity, existing))
                await WriteToHistoryAsync(existing);

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
        public new virtual void Delete(params object[] keyValues) {

            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {typeof(TEntity).Name} object with key value = {PrintKeys(keyValues)}");

            if (BeforeDelete(keyValues)) {
                if (WriteDelete(existing))
                    WriteToHistory(existing);
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
        public new virtual async Task DeleteAsync(params object[] keyValues){
            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {typeof(TEntity).Name} object with key value = {PrintKeys(keyValues)}");

            if (BeforeDelete(keyValues)) {
                if (WriteDelete(existing))
                    await WriteToHistoryAsync(existing);
                Context.Remove(existing);
                await Context.SaveChangesAsync();
                AfterDelete(keyValues);
                return;
            } else {
                Logger.LogDebug("For user {User}, call to BeforeDelete() returning false", ScopeProperties?.User ?? "");
                return;
            }
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

            var history = HistoryContext.Set<TEntity>()
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

            var history = HistoryContext.Set<TEntity>()
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


        public List<TEntity> GetByIdHistory(params object[] key) {
            var current = Context.Find<TEntity>(key);
            var primaryKeyPredicate = GetPrimaryKeyPredicate(current);

            var history = HistoryContext.Set<TEntity>()
                .Where(primaryKeyPredicate)
                .AsNoTracking()
                .ToList();

            var all = new List<TEntity> { current }.Union(history).ToList();
            return all;
        }



        public TEntity GetByIdAsOf(DateTime asOf, params object[] key) {
            var current = Context.Find<TEntity>(key);
            var primaryKeyPredicate = GetPrimaryKeyPredicate(current);
            var asOfPredicate = GetAsOfBetweenPredicate(asOf);

            var curr = Context.Set<TEntity>()
                .Where(primaryKeyPredicate)
                .Where(asOfPredicate)
                .AsNoTracking()
                .ToList();

            var history = HistoryContext.Set<TEntity>()
                .Where(primaryKeyPredicate)
                .Where(asOfPredicate)
                .AsNoTracking()
                .ToList();

            var all = curr.Union(history).FirstOrDefault();
            return all;
        }


        private Expression<Func<TEntity, bool>> GetPrimaryKeyPredicate(TEntity entity) {

            var state = Context.Entry(entity);
            var metadata = state.Metadata;
            var primaryKey = metadata.FindPrimaryKey();

            var pe = Expression.Parameter(typeof(TEntity), "e");
            Expression finalExpression = null;

            foreach (var pkProperty in primaryKey.Properties) {
                var type = typeof(TEntity);
                var left = Expression.Property(pe, type.GetProperty(pkProperty.Name));
                var right = Expression.Constant(pkProperty.GetGetter().GetClrValue(entity));
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

            var sysStartProp = GetSpecialProperty("SysStart");
            var sysEndProp = GetSpecialProperty("SysEnd");

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


        public virtual Expression<Func<TEntity, bool>> GetAsOfRangePredicate(DateTime from, DateTime to){

            var type = typeof(TEntity);

            var pe = Expression.Parameter(type, "e");

            var sysStartProp = GetSpecialProperty("SysStart");
            var sysEndProp = GetSpecialProperty("SysEnd");

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



        public virtual void WriteToHistory(TEntity existing){
            if (Context.Entry(existing).State != EntityState.Detached)
                Context.Entry(existing).State = EntityState.Detached;
            var sysEnd = GetSpecialPropertyValue(existing,"SysEnd");
            var sysUserNext = GetSpecialPropertyValue(existing, "SysUserNext");
            SetSpecialPropertyValue(existing,"SysEnd",DateTime.Now.AddTicks(-1));
            SetSpecialPropertyValue(existing, "SysUserNext",ScopeProperties.User);
            HistoryContext.Add(existing);
            HistoryContext.SaveChanges();
            HistoryContext.Entry(existing).State = EntityState.Detached;
            SetSpecialPropertyValue(existing, "SysEnd", sysEnd);
            SetSpecialPropertyValue(existing, "SysUserNext", sysUserNext);
        }


        public virtual async Task WriteToHistoryAsync(TEntity existing){
            var sysEnd = GetSpecialPropertyValue(existing, "SysEnd");
            var sysUserNext = GetSpecialPropertyValue(existing, "SysUserNext");
            SetSpecialPropertyValue(existing, "SysEnd", DateTime.Now.AddTicks(-1));
            SetSpecialPropertyValue(existing, "SysUserNext", ScopeProperties.User);
            HistoryContext.Add(existing);
            await HistoryContext.SaveChangesAsync();
            HistoryContext.Entry(existing).State = EntityState.Detached;
            SetSpecialPropertyValue(existing, "SysEnd", sysEnd);
            SetSpecialPropertyValue(existing, "SysUserNext", sysUserNext);
        }






    }
}
