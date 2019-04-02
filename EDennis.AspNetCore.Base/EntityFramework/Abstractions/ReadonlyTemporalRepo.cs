using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public abstract class ReadonlyTemporalRepo<TEntity, TContext, THistoryContext>
        : ReadonlyRepo<TEntity,TContext>
            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where THistoryContext: DbContext {


        public THistoryContext HistoryContext { get; set; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public ReadonlyTemporalRepo(TContext context, THistoryContext historyContext) :
            base(context) {
            HistoryContext = historyContext;
        }



        public List<TEntity> QueryAsOf(DateTime from,
                DateTime to, Expression<Func<TEntity, bool>> predicate,
                int pageNumber = 1, int pageSize = 10000,
                params Expression<Func<TEntity, dynamic>>[] orderSelectors
                ) {

            var asOfPredicate = GetAsOfRangePredicate(from, to);

            var current = Context.Query<TEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            var history = HistoryContext.Query<TEntity>()
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

            var current = Context.Query<TEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            var history = HistoryContext.Query<TEntity>()
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




    }
}

