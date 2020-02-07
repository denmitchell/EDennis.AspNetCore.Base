using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;

namespace Hr.RepoApi.Models {
    public class PersonRepo : Repo<Person, HrContext> {
        public PersonRepo(DbContextProvider<HrContext> provider, 
            IScopeProperties scopeProperties) 
            : base(provider, scopeProperties) {
        }
    }
}
