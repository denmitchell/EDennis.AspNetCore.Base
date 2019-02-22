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


        public List<TEntity> GetByIdHistory(params object[] key) {
            var current = Context.Find<TEntity>(key);
            var historyExpression = GetHistoryExpression(current);
            
            var history = HistoryContext.Set<TEntity>().Where(historyExpression).ToList();
            var all = new List<TEntity> { current }.Union(history).ToList();
            return all;
        }


        public List<TEntity> GetByIdAsOf(DateTime asOf, params object[] key) {
            var current = Context.Find<TEntity>(key);

            var between = GetAsOfBetweenExpression(current, asOf);
            var historyExpression = GetHistoryExpression(current, between);

            var history = HistoryContext.Set<TEntity>().Where(historyExpression).ToList();
            var all = new List<TEntity> { current }.Union(history).ToList();
            return all;
        }


        private Expression GetAsOfBetweenExpression(TEntity entity, DateTime asOf) {

            var pe = Expression.Parameter(typeof(TEntity), "e");

            var type = entity.GetType();

            var left1 = Expression.Property(pe, type.GetProperty("SysStart"));
            var right1 = Expression.Constant(asOf);
            var ge = Expression.GreaterThanOrEqual(left1, right1);

            var left2 = Expression.Property(pe, type.GetProperty("SysEnd"));
            var right2 = Expression.Constant(asOf);
            var le = Expression.LessThanOrEqual(left1, right1);

            var between = Expression.AndAlso(ge, le);

            return between;
        }


        private Expression<Func<TEntity, bool>> GetHistoryExpression(TEntity entity, Expression expressionToAdd = null) {

            var state = Context.Entry(entity);
            var metadata = state.Metadata;
            var primaryKey = metadata.FindPrimaryKey();

            /*
            var vals = props.Select(x => new { x.Name, Value = x.GetGetter().GetClrValue(entity) })
                .ToDictionary(y => y.Name, y => y.Value);
            var entityType = Context.Model.FindEntityType(typeof(TEntity));
            var primaryKey = entityType.FindPrimaryKey();
            */


            var pe = Expression.Parameter(typeof(TEntity), "e");
            Expression finalExpression = Expression.Constant(true);

            foreach (var pkProperty in primaryKey.Properties) {
                var type = pkProperty.ClrType;
                var left = Expression.Property(pe, type.GetProperty(pkProperty.Name));
                var right = Expression.Constant(pkProperty.GetGetter().GetClrValue(entity));
                var eq = Expression.Equal(left, right);
                finalExpression = Expression.AndAlso(finalExpression, eq);
            }

            if(expressionToAdd != null)
                finalExpression = Expression.AndAlso(finalExpression, expressionToAdd);

            return finalExpression as Expression<Func<TEntity,bool>>;
        }

    }

}

