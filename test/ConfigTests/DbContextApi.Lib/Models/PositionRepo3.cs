using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PositionRepo3 : SqlServerRepo<Position, DbContext3> {
        public PositionRepo3(DbContext3 context, IScopeProperties scopeProperties, ILogger<Repo<Position, DbContext3>> logger, IScopedLogger scopedLogger) : base(context, scopeProperties, logger, scopedLogger) {
        }
    }
}
