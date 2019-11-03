using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface IRepo<TEntity, TContext>
        where TEntity : class, new()
        where TContext : DbContext {
        TContext Context { get; set; }
        ILogger Logger { get; }
        IQueryable<TEntity> Query { get; }
        ScopeProperties ScopeProperties { get; set; }

        TEntity Create(TEntity entity);
        Task<TEntity> CreateAsync(TEntity entity);
        void Delete(params object[] keyValues);
        Task DeleteAsync(params object[] keyValues);
        bool Exists(params object[] keyValues);
        Task<bool> ExistsAsync(params object[] keyValues);
        TEntity GetById(params object[] keyValues);
        Task<TEntity> GetByIdAsync(params object[] keyValues);
        System.Collections.Generic.List<dynamic> GetFromDynamicLinq(string where = null, string orderBy = null, string select = null, int? skip = null, int? take = null);
        Task<System.Collections.Generic.List<dynamic>> GetFromDynamicLinqAsync(string where = null, string orderBy = null, string select = null, int? skip = null, int? take = null);
        TEntity Update(dynamic partialEntity, params object[] keyValues);
        TEntity Update(TEntity entity, params object[] keyValues);
        Task<TEntity> UpdateAsync(dynamic partialEntity, params object[] keyValues);
        Task<TEntity> UpdateAsync(TEntity entity, params object[] keyValues);

        void Check(string checkLabel, dynamic pre, dynamic post, Func<dynamic, dynamic, bool> isExpectedChange);
        void Check(DynamicLinqParameters check);
    }
}