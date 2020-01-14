using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PositionRepo3 : Repo<Position, DbContext3> {
        public PositionRepo3(DbContextProvider<DbContext3> provider, IScopeProperties scopeProperties, ILogger<Repo<Position, DbContext3>> logger, IScopedLogger scopedLogger) : base(provider, scopeProperties, logger, scopedLogger) {
        }
    }
}
