using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface IRepo<TEntity, TContext>
        where TEntity : class, IHasSysUser, IHasLongId, new()
        where TContext : DbContext {
        TContext Context { get; set; }
        ILogger Logger { get; }
        IQueryable<TEntity> Query { get; }
        ScopeProperties ScopeProperties { get; set; }

        void AfterCreate(TEntity inputEntity, TEntity resultEntity);
        void AfterDelete(TEntity deletedEntity, params object[] keyValues);
        void AfterUpdate(dynamic inputEntity, TEntity resultEntity, params object[] keyValues);
        bool BeforeCreate(TEntity inputEntity);
        bool BeforeDelete(params object[] keyValues);
        bool BeforeUpdate(dynamic inputEntity, params object[] keyValues);
        void CheckChange(string checkContext, dynamic pre, DynamicLinqParameters postStateParameters, System.Func<dynamic, dynamic, bool> isExpectedChange);
        Task CheckChangeAsync(string checkContext, dynamic pre, DynamicLinqParameters postStateParameters, System.Func<dynamic, dynamic, bool> isExpectedChange);
        dynamic CheckPreState(string checkContext, DynamicLinqParameters preStateParameters, System.Func<dynamic, bool> isExpectedPreState);
        Task<dynamic> CheckPreStateAsync(string checkContext, DynamicLinqParameters preStateParameters, System.Func<dynamic, bool> isExpectedPreState);
        TEntity Create(TEntity inputEntity);
        Task<TEntity> CreateAsync(TEntity inputEntity);
        void Delete(params object[] keyValues);
        Task DeleteAsync(params object[] keyValues);
        TEntity Execute(Operation operation, dynamic entity, params object[] keyValues);
        Task<TEntity> ExecuteAsync(Operation operation, dynamic entity, params object[] keyValues);
        bool Exists(params object[] keyValues);
        Task<bool> ExistsAsync(params object[] keyValues);
        TEntity GetById(params object[] keyValues);
        Task<TEntity> GetByIdAsync(params object[] keyValues);
        List<dynamic> GetFromDynamicLinq(string where = null, string orderBy = null, string select = null, int? skip = null, int? take = null);
        List<dynamic> GetFromDynamicLinq(DynamicLinqParameters parms);
        Task<List<dynamic>> GetFromDynamicLinqAsync(string where = null, string orderBy = null, string select = null, int? skip = null, int? take = null);
        Task<List<dynamic>> GetFromDynamicLinqAsync(DynamicLinqParameters parms);
        Dictionary<string, long> GetNextCompoundIdBlock();
        TEntity Update(dynamic partialEntity, params object[] keyValues);
        TEntity Update(TEntity inputEntity, params object[] keyValues);
        Task<TEntity> UpdateAsync(dynamic partialEntity, params object[] keyValues);
        Task<TEntity> UpdateAsync(TEntity inputEntity, params object[] keyValues);
    }
}