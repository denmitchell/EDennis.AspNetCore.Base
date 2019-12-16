using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DefaultPoliciesConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase {

        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger) {
            _logger = logger;
        }

        [HttpGet("Admin")]
        public string GetAdmin() {
            return "Hello, Admin!";
        }

        [HttpGet("User")]
        public string GetUser() {
            return "Hello, User!";
        }
    }
}
