using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;

namespace Hr.Api.Models {
    public class AddressRepo : Repo<Address, HrContext> {
        public AddressRepo(DbContextProvider<HrContext> provider, 
            IScopeProperties scopeProperties) 
            : base(provider, scopeProperties) {
        }
    }
}
