using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PositionRepo1 : Repo<Position, DbContext1> {
        public PositionRepo1(DbContextProvider<DbContext1> provider, IScopeProperties scopeProperties, ILogger<Repo<Position, DbContext1>> logger, IScopedLogger scopedLogger) : base(provider, scopeProperties, logger, scopedLogger) {
        }
    }
}
