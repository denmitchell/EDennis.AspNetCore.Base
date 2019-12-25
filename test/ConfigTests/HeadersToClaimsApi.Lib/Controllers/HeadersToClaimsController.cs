using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.HeadersToClaimsConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class HeadersToClaimsController : ControllerBase {

        private readonly ILogger<HeadersToClaimsController> _logger;
        private readonly IOptionsMonitor<HeadersToClaims> _headersToClaims;

        public HeadersToClaimsController(IOptionsMonitor<HeadersToClaims> headersToClaims, ILogger<HeadersToClaimsController> logger) {
            _logger = logger;
            _headersToClaims = headersToClaims;
        }

        [HttpGet]
        public HeadersToClaims Get() {
            return _headersToClaims.CurrentValue;
        }
    }
}
