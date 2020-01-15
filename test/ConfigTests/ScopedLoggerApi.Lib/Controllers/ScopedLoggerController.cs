using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.ScopedLoggerApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ScopedLoggerController : ControllerBase {
        private readonly IOptionsMonitor<ScopedTraceLoggerSettings> _uls;

        public ScopedLoggerController(
            IOptionsMonitor<ScopedTraceLoggerSettings> uls) {
            _uls = uls;
        }

        [HttpGet]
        public ScopedTraceLoggerSettings Get() {
            return _uls.CurrentValue;
        }
    }
}
