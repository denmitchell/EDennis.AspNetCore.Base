using EDennis.AspNetCore.Base.Web;
using Hr.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AddressController: RepoController<Address,HrContext,AddressRepo>{
        public AddressController(AddressRepo repo) : base(repo) { }
    }
}
