using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PositionRepo2 : SqlServerRepo<Position, DbContext2> {
        public PositionRepo2(DbContextProvider<DbContext2> provider, IScopeProperties scopeProperties, ILogger<Repo<Position, DbContext2>> logger, IScopedLogger scopedLogger) : base(provider, scopeProperties, logger, scopedLogger) {
        }
    }
}
