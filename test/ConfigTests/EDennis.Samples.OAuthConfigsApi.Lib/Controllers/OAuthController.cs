using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.OAuthConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class OAuthController : ControllerBase {

        private readonly ILogger<OAuthController> _logger;

        public OAuthController(
            //visually inspect jwtBearerOptions.Action.Target.settings in Debugger, Autos
            IConfigureOptions<JwtBearerOptions> jwtBearerOptions,

            ILogger<OAuthController> logger
            ) {
            _logger = logger;
        }

        [Authorize]
        [HttpGet("Secure")]
        public string GetSecure() {
            return "Secure";
        }

        [HttpGet("NonSecure")]
        public string GetNonSecure() {
            return "NonSecure";
        }
    }
}
