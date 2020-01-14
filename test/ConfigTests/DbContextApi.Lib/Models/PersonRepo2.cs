using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PersonRepo2 : Repo<Person, DbContext2> {
        public PersonRepo2(DbContextProvider<DbContext2> provider, IScopeProperties scopeProperties, ILogger<Repo<Person, DbContext2>> logger, IScopedLogger scopedLogger) : base(provider, scopeProperties, logger, scopedLogger) {
        }
    }
}
