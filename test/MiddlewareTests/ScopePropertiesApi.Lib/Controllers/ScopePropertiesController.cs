using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.ScopePropertiesMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ScopePropertiesController : ControllerBase {

        private readonly ILogger<ScopePropertiesController> _logger;

        public ScopePropertiesController(IScopeProperties scopeProperties, ILogger<ScopePropertiesController> logger) {
            ScopeProperties = scopeProperties;
            _logger = logger;
        }

        public IScopeProperties ScopeProperties { get; }

        [HttpGet]
        public IScopeProperties Get() {
            return ScopeProperties;
        }
    }
}
