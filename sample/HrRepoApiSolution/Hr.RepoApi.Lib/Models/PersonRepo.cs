using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;

namespace Hr.Api.Models {
    public class PersonRepo : Repo<Person, HrContext> {
        public PersonRepo(DbContextProvider<HrContext> provider, 
            IScopeProperties scopeProperties) 
            : base(provider, scopeProperties) {
        }
    }
}
