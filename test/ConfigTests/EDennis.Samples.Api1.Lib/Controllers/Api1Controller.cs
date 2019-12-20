using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Api1.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class Api1Controller : ControllerBase {
        private readonly ILogger<Api1Controller> _logger;

        public Api1Controller(ILogger<Api1Controller> logger) {
            _logger = logger;
        }

        [HttpGet]
        public string Get() {
            return "Hello, World!";
        }
    }
}
