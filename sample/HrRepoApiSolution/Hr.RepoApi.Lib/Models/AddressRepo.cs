using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;

namespace Hr.RepoApi.Models {
    public class AddressRepo : Repo<Address, HrContext> {
        public AddressRepo(DbContextProvider<HrContext> provider, 
            IScopeProperties scopeProperties) 
            : base(provider, scopeProperties) {
        }
    }
}
