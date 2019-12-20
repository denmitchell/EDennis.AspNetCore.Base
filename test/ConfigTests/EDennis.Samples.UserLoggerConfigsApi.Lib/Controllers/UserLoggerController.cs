using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.UserLoggerConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UserLoggerController : ControllerBase {
        private readonly IOptionsMonitor<UserLoggerSettings> _uls;
        private readonly ILogger<UserLoggerController> _logger;

        public UserLoggerController(
            IOptionsMonitor<UserLoggerSettings> uls,
            ILogger<UserLoggerController> logger) {
            _uls = uls;
            _logger = logger;
        }

        [HttpGet]
        public UserLoggerSettings Get() {
            return _uls.CurrentValue;
        }
    }
}
