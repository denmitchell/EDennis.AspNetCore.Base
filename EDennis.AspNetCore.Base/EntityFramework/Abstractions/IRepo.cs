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

        void AfterCreate(TEntity entity);
        void AfterDelete(params object[] keyValues);
        void AfterUpdate(dynamic entityOrPartialEntity, params object[] keyValues);
        bool BeforeCreate(TEntity entity);
        bool BeforeDelete(params object[] keyValues);
        bool BeforeUpdate(dynamic entityOrPartialEntity, params object[] keyValues);
        void CheckChange(string checkContext, dynamic pre, DynamicLinqParameters postStateParameters, System.Func<dynamic, dynamic, bool> isExpectedChange);
        Task CheckChangeAsync(string checkContext, dynamic pre, DynamicLinqParameters postStateParameters, System.Func<dynamic, dynamic, bool> isExpectedChange);
        dynamic CheckPreState(string checkContext, DynamicLinqParameters preStateParameters, System.Func<dynamic, bool> isExpectedPreState);
        Task<dynamic> CheckPreStateAsync(string checkContext, DynamicLinqParameters preStateParameters, System.Func<dynamic, bool> isExpectedPreState);
        TEntity Create(TEntity entity);
        Task<TEntity> CreateAsync(TEntity entity);
        void Delete(params object[] keyValues);
        Task DeleteAsync(params object[] keyValues);
        bool Exists(params object[] keyValues);
        Task<bool> ExistsAsync(params object[] keyValues);
        TEntity GetById(params object[] keyValues);
        Task<TEntity> GetByIdAsync(params object[] keyValues);
        System.Collections.Generic.List<dynamic> GetFromDynamicLinq(string where = null, string orderBy = null, string select = null, int? skip = null, int? take = null);
        System.Collections.Generic.List<dynamic> GetFromDynamicLinq(DynamicLinqParameters parms);
        Task<System.Collections.Generic.List<dynamic>> GetFromDynamicLinqAsync(string where = null, string orderBy = null, string select = null, int? skip = null, int? take = null);
        Task<System.Collections.Generic.List<dynamic>> GetFromDynamicLinqAsync(DynamicLinqParameters parms);
        TEntity SafeCreate(DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, TEntity entity);
        TEntity SafeCreate<TCheckEntity, TCheckContext>(IRepo<TCheckEntity, TCheckContext> checkRepo, DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, TEntity entity)
            where TCheckEntity : class, new()
            where TCheckContext : DbContext;
        Task<TEntity> SafeCreateAsync(DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, TEntity entity);
        Task<TEntity> SafeCreateAsync<TCheckEntity, TCheckContext>(IRepo<TCheckEntity, TCheckContext> checkRepo, DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, TEntity entity)
            where TCheckEntity : class, new()
            where TCheckContext : DbContext;
        void SafeDelete(DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, params object[] keyValues);
        void SafeDelete<TCheckEntity, TCheckContext>(IRepo<TCheckEntity, TCheckContext> checkRepo, DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, params object[] keyValues)
            where TCheckEntity : class, new()
            where TCheckContext : DbContext;
        Task SafeDeleteAsync(DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, params object[] keyValues);
        Task SafeDeleteAsync<TCheckEntity, TCheckContext>(IRepo<TCheckEntity, TCheckContext> checkRepo, DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, params object[] keyValues)
            where TCheckEntity : class, new()
            where TCheckContext : DbContext;
        TEntity SafeUpdate(DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, dynamic partialEntity, params object[] keyValues);
        TEntity SafeUpdate(DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, TEntity entity, params object[] keyValues);
        TEntity SafeUpdate<TCheckEntity, TCheckContext>(IRepo<TCheckEntity, TCheckContext> checkRepo, DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, dynamic partialEntity, params object[] keyValues)
            where TCheckEntity : class, new()
            where TCheckContext : DbContext;
        TEntity SafeUpdate<TCheckEntity, TCheckContext>(IRepo<TCheckEntity, TCheckContext> checkRepo, DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, TEntity entity, params object[] keyValues)
            where TCheckEntity : class, new()
            where TCheckContext : DbContext;
        Task<TEntity> SafeUpdateAsync(DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, dynamic partialEntity, params object[] keyValues);
        Task<TEntity> SafeUpdateAsync(DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, TEntity entity, params object[] keyValues);
        Task<TEntity> SafeUpdateAsync<TCheckEntity, TCheckContext>(IRepo<TCheckEntity, TCheckContext> checkRepo, DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, dynamic partialEntity, params object[] keyValues)
            where TCheckEntity : class, new()
            where TCheckContext : DbContext;
        Task<TEntity> SafeUpdateAsync<TCheckEntity, TCheckContext>(IRepo<TCheckEntity, TCheckContext> checkRepo, DynamicLinqParameters preState, System.Func<dynamic, bool> isExpectedPreState, DynamicLinqParameters postState, System.Func<dynamic, dynamic, bool> isExpectedChange, int timeoutInSeconds, bool isTestOnly, TEntity entity, params object[] keyValues)
            where TCheckEntity : class, new()
            where TCheckContext : DbContext;
        TEntity Update(dynamic partialEntity, params object[] keyValues);
        TEntity Update(TEntity entity, params object[] keyValues);
        Task<TEntity> UpdateAsync(dynamic partialEntity, params object[] keyValues);
        Task<TEntity> UpdateAsync(TEntity entity, params object[] keyValues);
    }
}