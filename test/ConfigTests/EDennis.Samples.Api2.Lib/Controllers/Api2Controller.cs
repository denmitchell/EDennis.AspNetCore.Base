using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Api2.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class Api2Controller : ControllerBase {
        private readonly ILogger<Api2Controller> _logger;

        public Api2Controller(ILogger<Api2Controller> logger) {
            _logger = logger;
        }

        [HttpGet]
        public string Get() {
            return "Hello, World!";
        }
    }
}
