using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi {
    public class PositionRepo : SqlServerRepo<Position, AppDbContext> {
        public PositionRepo(AppDbContext context, IScopeProperties scopeProperties, ILogger<Repo<Position, DbContext3>> logger, IScopedLogger scopedLogger) : base(context, scopeProperties, logger, scopedLogger) {
        }
    }
}
