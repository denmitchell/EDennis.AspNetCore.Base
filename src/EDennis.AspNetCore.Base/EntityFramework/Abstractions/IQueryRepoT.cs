using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface IQueryRepo<TEntity, TContext> : IRepo
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext {
        TContext Context { get; set; }
        IQueryable<TEntity> Query { get; }
        IScopeProperties ScopeProperties { get; set; }
        DynamicLinqResult GetWithDynamicLinq(string select, string where = null, string orderBy = null, int? skip = null, int? take = null, int? totalRecords = null);
        Task<DynamicLinqResult> GetWithDynamicLinqAsync(string select, string where = null, string orderBy = null, int? skip = null, int? take = null, int? totalRecords = null);
        DynamicLinqResult<TEntity> GetWithDynamicLinq(string where = null, string orderBy = null, int? skip = null, int? take = null, int? totalRecords = null);
        Task<DynamicLinqResult<TEntity>> GetWithDynamicLinqAsync(string where = null, string orderBy = null, int? skip = null, int? take = null, int? totalRecords = null);
    }
}