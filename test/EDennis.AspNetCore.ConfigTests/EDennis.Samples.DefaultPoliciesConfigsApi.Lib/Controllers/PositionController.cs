using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DefaultPoliciesConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PositionController : ControllerBase {

        private readonly ILogger<PositionController> _logger;

        public PositionController(ILogger<PositionController> logger) {
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
