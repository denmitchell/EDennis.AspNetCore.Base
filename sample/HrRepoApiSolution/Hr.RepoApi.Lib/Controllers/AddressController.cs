using EDennis.AspNetCore.Base.Web;
using Hr.RepoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hr.RepoApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController: RepoController<Address,HrContext,AddressRepo>{
        public AddressController(AddressRepo repo) : base(repo) { }
    }
}
