using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PositionRepo1 : SqlServerRepo<Position, DbContext1> {
        public PositionRepo1(DbContext1 context, IScopeProperties scopeProperties, ILogger<Repo<Position, DbContext1>> logger, IScopedLogger scopedLogger) : base(context, scopeProperties, logger, scopedLogger) {
        }
    }
}
