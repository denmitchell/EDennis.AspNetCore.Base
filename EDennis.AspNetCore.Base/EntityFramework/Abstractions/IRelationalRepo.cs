using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface IRelationalRepo<TEntity> where TEntity : class, new() {
        List<TEntity> GetFromSql(string sql);
        Task<List<TEntity>> GetFromSqlAsync(string sql);
        TScalarType GetScalarFromSql<TScalarType>(string sql);
        Task<TScalarType> GetScalarFromSqlAsync<TScalarType>(string sql);
    }
}