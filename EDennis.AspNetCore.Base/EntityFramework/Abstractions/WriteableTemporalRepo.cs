using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
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
    public abstract class WriteableTemporalRepo<TEntity, TContext, THistoryContext> 
                : WriteableRepo<TEntity,TContext>, ITemporalRepo<TEntity, TContext, THistoryContext>
            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where THistoryContext : DbContext {


        public virtual bool WriteUpdate(TEntity next, TEntity current)
            => (DateTime.Now > current.SysStart.AddHours(8)
                || SysUser != current.SysUser);


        public virtual bool WriteDelete(TEntity current)
            => (DateTime.Now > current.SysStart.AddHours(8)
                || SysUser != current.SysUser);


        public THistoryContext HistoryContext { get; set; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public WriteableTemporalRepo(TContext context, THistoryContext historyContext,
            ScopeProperties scopeProperties) : base(context,scopeProperties) {
            HistoryContext = historyContext;
        }


        /// <summary>
        /// Creates a new entity from the provided input
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity</returns>
        public override TEntity Create(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot create a null {entity.GetType().Name}");

            entity.SysStart = DateTime.Now;
            entity.SysEnd = DateTime.MaxValue;
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
        public override async Task<TEntity> CreateAsync(TEntity entity) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot create a null {entity.GetType().Name}");

            entity.SysStart = DateTime.Now;
            entity.SysEnd = DateTime.MaxValue;
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
        public override TEntity Update(TEntity entity, params object[] keyValues) {
            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            var existing = Context.Find<TEntity>(keyValues);

            if (WriteUpdate(entity, existing))
                WriteToHistory(existing);

            entity.SysStart = DateTime.Now;
            entity.SysEnd = DateTime.MaxValue;
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
        public override async Task<TEntity> UpdateAsync(TEntity entity, params object[] keyValues) {

            if (entity == null)
                throw new MissingEntityException(
                    $"Cannot update a null {entity.GetType().Name}");

            var existing = await Context.FindAsync<TEntity>(keyValues);

            if (WriteUpdate(entity, existing))
                await WriteToHistoryAsync(existing);

            entity.SysStart = DateTime.Now;
            entity.SysEnd = DateTime.MaxValue;
            entity.SysUser = ScopeProperties.User;
            Context.Update(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// Deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public override void Delete(params object[] keyValues) {

            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {new TEntity().GetType().Name} object with key value = {PrintKeys(keyValues)}");

            if (WriteDelete(existing))
                WriteToHistory(existing);

            Context.Remove(existing);
            Context.SaveChanges();

        }

        /// <summary>
        /// Asynchrously deletes the entity whose primary keys match the provided input
        /// </summary>
        /// <param name="keyValues">The primary key as key-value object array</param>
        public override async Task DeleteAsync(params object[] keyValues) {
            var existing = Context.Find<TEntity>(keyValues);
            if (existing == null)
                throw new MissingEntityException(
                    $"Cannot find {new TEntity().GetType().Name} object with key value = {PrintKeys(keyValues)}");

            if (WriteDelete(existing))
                await WriteToHistoryAsync(existing);

            Context.Remove(existing);
            await Context.SaveChangesAsync();
        }


        public List<TEntity> QueryAsOf(DateTime from,
                DateTime to, Expression<Func<TEntity, bool>> predicate,
                int pageNumber = 1, int pageSize = 10000,
                params Expression<Func<TEntity, dynamic>>[] orderSelectors
                ) {

            var asOfPredicate = GetAsOfRangePredicate(from,to);

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
                Expression<Func<TEntity,bool>> predicate,
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



        public List<TEntity> GetByIdAsOf(DateTime asOf, params object[] key) {
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

            var all = curr.Union(history).ToList();
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



        private Expression<Func<TEntity, bool>> GetAsOfBetweenPredicate(DateTime asOf) {

            var type = typeof(TEntity);

            var pe = Expression.Parameter(type, "e");


            var left1 = Expression.Property(pe, type.GetProperty("SysStart"));
            var right1 = Expression.Constant(asOf);
            var ge = Expression.LessThanOrEqual(left1, right1);

            var left2 = Expression.Property(pe, type.GetProperty("SysEnd"));
            var right2 = Expression.Constant(asOf);
            var le = Expression.GreaterThanOrEqual(left2, right2);


            var between = Expression.AndAlso(ge, le);

            var expr = Expression.Lambda<Func<TEntity, bool>>(between, pe);

            return expr;
        }


        private Expression<Func<TEntity, bool>> GetAsOfRangePredicate(DateTime from, DateTime to) {

            var type = typeof(TEntity);

            var pe = Expression.Parameter(type, "e");


            var left1 = Expression.Property(pe, type.GetProperty("SysStart"));
            var right1 = Expression.Constant(to);
            var ge = Expression.LessThanOrEqual(left1, right1);

            var left2 = Expression.Property(pe, type.GetProperty("SysEnd"));
            var right2 = Expression.Constant(from);
            var le = Expression.GreaterThanOrEqual(left2, right2);

            var between = Expression.AndAlso(ge, le);

            var expr = Expression.Lambda<Func<TEntity, bool>>(between, pe);

            return expr;
        }




        private void WriteToHistory(TEntity existing) {
            if (Context.Entry(existing).State != EntityState.Detached)
                Context.Entry(existing).State = EntityState.Detached;
            existing.SysEnd = DateTime.Now.AddTicks(-1);
            existing.SysUserNext = ScopeProperties.User;
            HistoryContext.Add(existing);
            HistoryContext.SaveChanges();
            HistoryContext.Entry(existing).State = EntityState.Detached;
        }


        private async Task WriteToHistoryAsync(TEntity existing) {
            if (Context.Entry(existing).State != EntityState.Detached)
                Context.Entry(existing).State = EntityState.Detached;
            existing.SysEnd = DateTime.Now.AddTicks(-1);
            existing.SysUserNext = ScopeProperties.User;
            HistoryContext.Add(existing);
            await HistoryContext.SaveChangesAsync();
            HistoryContext.Entry(existing).State = EntityState.Detached;
        }


    }


}

