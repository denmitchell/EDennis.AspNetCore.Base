using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;

namespace Hr.Api.Models {
    public class AddressRepo : Repo<Address, HrContext> {
        public AddressRepo(DbContextProvider<HrContext> provider, 
            IScopeProperties scopeProperties, 
            ILogger<Repo<Address, HrContext>> logger) 
            : base(provider, scopeProperties, logger) {
        }
    }
}
