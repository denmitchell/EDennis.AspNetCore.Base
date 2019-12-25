using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PositionRepo2 : SqlServerRepo<Position, DbContext2> {
        public PositionRepo2(DbContext2 context, IScopeProperties scopeProperties, ILogger<Repo<Position, DbContext2>> logger, IScopedLogger scopedLogger) : base(context, scopeProperties, logger, scopedLogger) {
        }
    }
}
