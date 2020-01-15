using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextConfigsApi.Lib {
    public class PersonRepo1 : Repo<Person, DbContext1> {
        public PersonRepo1(DbContextProvider<DbContext1> provider, IScopeProperties scopeProperties, ILogger<Repo<Person, DbContext1>> logger) : base(provider, scopeProperties, logger) {
        }
    }
}
