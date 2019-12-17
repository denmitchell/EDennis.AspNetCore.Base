using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.HeadersToClaimsMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase {

        private readonly ILogger<ClaimsController> _logger;

        public ClaimsController(ILogger<ClaimsController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<SimpleClaim> Get() {
            var claims = HttpContext.User.Claims;
            return claims.Select(x => new SimpleClaim {Type = x.Type, Value = x.Value});
        }
    }
}
