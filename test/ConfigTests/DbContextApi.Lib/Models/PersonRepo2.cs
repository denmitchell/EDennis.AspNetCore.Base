using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PersonRepo2 : SqlServerRepo<Person, DbContext2> {
        public PersonRepo2(DbContext2 context, IScopeProperties scopeProperties, ILogger<Repo<Person, DbContext2>> logger, IScopedLogger scopedLogger) : base(context, scopeProperties, logger, scopedLogger) {
        }
    }
}
