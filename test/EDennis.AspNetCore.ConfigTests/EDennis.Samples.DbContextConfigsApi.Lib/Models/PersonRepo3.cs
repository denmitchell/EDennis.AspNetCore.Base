using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PersonRepo3 : SqlServerRepo<Person, DbContext3> {
        public PersonRepo3(DbContext3 context, IScopeProperties scopeProperties, ILogger<Repo<Person, DbContext3>> logger, IScopedLogger scopedLogger) : base(context, scopeProperties, logger, scopedLogger) {
        }
    }
}
