using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface ISqlServerRepo {
        string GetFromJsonSql(string fromJsonSql);
        Task<string> GetFromJsonSqlAsync(string fromJsonSql);
        dynamic GetFromStoredProcedure(string spName, IEnumerable<KeyValuePair<string, string>> parms);
        Task<dynamic> GetFromStoredProcedureAsync(string spName, IEnumerable<KeyValuePair<string, string>> parms);
        string GetJsonColumnFromStoredProcedure(string spName, IEnumerable<KeyValuePair<string, string>> parms);
        Task<string> GetJsonColumnFromStoredProcedureAsync(string spName, IEnumerable<KeyValuePair<string, string>> parms);
    }
}