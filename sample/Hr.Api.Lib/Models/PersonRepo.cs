using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace Hr.Api.Models {
    public class PersonRepo : Repo<Person, HrContext> {

        public PersonRepo(DbContextProvider<HrContext> provider, 
            IScopeProperties scopeProperties, 
            ILogger<Repo<Person, HrContext>> logger) 
            : base(provider, scopeProperties, logger) {
        }
    }
}
