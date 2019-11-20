using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext> : IRepo<TEntity, TContext>
where TEntity : class, IEFCoreTemporalModel, new()
        where THistoryEntity : TEntity
        where TContext : DbContext
        where THistoryContext : DbContext {
        THistoryContext HistoryContext { get; set; }

        Expression<Func<TEntity, bool>> GetAsOfBetweenPredicate(DateTime asOf);
        Expression<Func<TEntity, bool>> GetAsOfRangePredicate(DateTime from, DateTime to);
        TEntity GetByIdAsOf(DateTime asOf, params object[] key);
        List<TEntity> GetByIdHistory(params object[] key);
        List<TEntity> QueryAsOf(DateTime from, DateTime to, Expression<Func<TEntity, bool>> predicate, int pageNumber = 1, int pageSize = 10000, params Expression<Func<TEntity, dynamic>>[] orderSelectors);
        List<TEntity> QueryAsOf(DateTime asOf, Expression<Func<TEntity, bool>> predicate, int pageNumber = 1, int pageSize = 10000, params Expression<Func<TEntity, dynamic>>[] orderSelectors);
        bool WriteDelete(TEntity current);
        void WriteToHistory(TEntity existing);
        Task WriteToHistoryAsync(TEntity existing);
        bool WriteUpdate(dynamic next, TEntity current);
        bool WriteUpdate(TEntity next, TEntity current);
    }
}