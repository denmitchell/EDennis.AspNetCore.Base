using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PersonRepo3 : Repo<Person, DbContext3> {
        public PersonRepo3(DbContextProvider<DbContext3> provider, IScopeProperties scopeProperties, ILogger<Repo<Person, DbContext3>> logger, IScopedLogger scopedLogger) : base(provider, scopeProperties, logger, scopedLogger) {
        }
    }
}
