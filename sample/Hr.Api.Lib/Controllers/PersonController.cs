using EDennis.AspNetCore.Base.Web;
using Hr.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hr.Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PersonController
        : SqlServerWriteableController<Person, HrContext> {

        public PersonController(PersonRepo repo, ILogger<PersonController> logger)
            : base(repo, logger) { }

    }
}
