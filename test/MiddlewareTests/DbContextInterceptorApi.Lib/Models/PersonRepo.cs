using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi {
    public class PersonRepo : SqlServerRepo<Person, AppDbContext> {
        public PersonRepo(AppDbContext context, IScopeProperties scopeProperties, ILogger<Repo<Person, AppDbContext>> logger, IScopedLogger scopedLogger) : base(context, scopeProperties, logger, scopedLogger) {
        }
    }
}
