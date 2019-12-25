using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.ScopedLoggerApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ScopedLoggerController : ControllerBase {
        private readonly IOptionsMonitor<ScopedLoggerSettings> _uls;
        private readonly ILogger<ScopedLoggerController> _logger;

        public ScopedLoggerController(
            IOptionsMonitor<ScopedLoggerSettings> uls,
            ILogger<ScopedLoggerController> logger) {
            _uls = uls;
            _logger = logger;
        }

        [HttpGet]
        public ScopedLoggerSettings Get() {
            return _uls.CurrentValue;
        }
    }
}
