using EDennis.AspNetCore.Base.Web;
using Hr.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hr.Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AddressController 
        : SqlServerWriteableController<Address,HrContext>{

        public AddressController(AddressRepo repo, ILogger<AddressController> logger) 
            : base(repo, logger) { }

    }
}
