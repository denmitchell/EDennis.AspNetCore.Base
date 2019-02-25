using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {

    public interface ITemporalRepo : IRepo { }

    public interface ITemporalRepo<TEntity, TContext,THistoryContext>
        : ITemporalRepo
        where TEntity : class, IEFCoreTemporalModel, new()
        where TContext : DbContext
        where THistoryContext : DbContext {

        TContext Context { get; set; }
        THistoryContext HistoryContext { get; set; }
        
    }

    public static class ITemporalRepoExtensions {


        public static List<TEntity> GetByIdHistory<TEntity,TContext,THistoryContext>(
            this ITemporalRepo<TEntity,TContext,THistoryContext> repo,
            params object[] key)
        where TEntity : class, IEFCoreTemporalModel, new()
        where TContext : DbContext
        where THistoryContext : DbContext {
            var current = repo.Context.Find<TEntity>(key);
            var historyExpression = GetHistoryExpression(repo.Context, current);

            var history = repo.HistoryContext.Query<TEntity>().Where(historyExpression).ToList();
            var all = new List<TEntity> { current }.Union(history).ToList();
            return all;
        }


        public static List<TEntity> GetByIdAsOf<TEntity, TContext, THistoryContext>(
            this ITemporalRepo<TEntity, TContext, THistoryContext> repo, 
            DateTime asOf, params object[] key)
        where TEntity : class, IEFCoreTemporalModel, new()
        where TContext : DbContext
        where THistoryContext : DbContext {
            var current = repo.Context.Find<TEntity>(key);

            var between = GetAsOfBetweenExpression(current, asOf);
            var historyExpression = GetHistoryExpression(repo.Context,current, between);

            var history = repo.HistoryContext.Query<TEntity>().Where(historyExpression).ToList();
            var all = new List<TEntity> { current }.Union(history).ToList();
            return all;
        }


        private static Expression GetAsOfBetweenExpression<TEntity>(TEntity entity, DateTime asOf)
            where TEntity: class, IEFCoreTemporalModel, new() {

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


        private static Expression<Func<TEntity, bool>> GetHistoryExpression<TEntity, TContext>(
            TContext context, TEntity entity, Expression expressionToAdd = null)
            where TContext : DbContext
            where TEntity : class, IEFCoreTemporalModel, new() {

            var state = context.Entry(entity);
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

            if (expressionToAdd != null)
                finalExpression = Expression.AndAlso(finalExpression, expressionToAdd);

            return finalExpression as Expression<Func<TEntity, bool>>;
        }


    }


}
