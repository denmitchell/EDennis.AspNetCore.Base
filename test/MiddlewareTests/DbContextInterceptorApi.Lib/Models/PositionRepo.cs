using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi {
    public class PositionRepo : Repo<Position, AppDbContext> {
        public PositionRepo(DbContextProvider<AppDbContext> provider, IScopeProperties scopeProperties, ILogger<Repo<Position, AppDbContext>> logger, IScopedLogger scopedLogger) : base(provider, scopeProperties, logger, scopedLogger) {
        }
    }
}
