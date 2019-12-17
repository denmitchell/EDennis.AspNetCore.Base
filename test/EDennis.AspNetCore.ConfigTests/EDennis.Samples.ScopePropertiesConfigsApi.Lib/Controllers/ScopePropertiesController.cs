using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.ScopePropertiesConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ScopePropertiesController : ControllerBase {

        private readonly IOptionsMonitor<ScopePropertiesSettings> _sps;
        private readonly ILogger<ScopePropertiesController> _logger;

        public ScopePropertiesController(
            IOptionsMonitor<ScopePropertiesSettings> sps,
            ILogger<ScopePropertiesController> logger) {
            _sps = sps;
            _logger = logger;
        }

        [HttpGet]
        public ScopePropertiesSettings Get() {
            return _sps.CurrentValue;
        }
    }
}
