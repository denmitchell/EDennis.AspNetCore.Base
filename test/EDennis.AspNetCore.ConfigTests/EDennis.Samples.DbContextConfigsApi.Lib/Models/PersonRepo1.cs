using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PersonRepo1 : SqlServerRepo<Person, DbContext1> {
        public PersonRepo1(DbContext1 context, IScopeProperties scopeProperties, ILogger<Repo<Person, DbContext1>> logger, IScopedLogger scopedLogger) : base(context, scopeProperties, logger, scopedLogger) {
        }
    }
}
