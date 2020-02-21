using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace DbContextInterceptorApi {
    public class PositionRepo : Repo<Position, AppDbContext> {
        public PositionRepo(DbContextProvider<AppDbContext> provider, IScopeProperties scopeProperties) : base(provider, scopeProperties) {
        }
    }
}
