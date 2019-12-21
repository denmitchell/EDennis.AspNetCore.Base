using EDennis.AspNetCore.Base;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.ScopePropertiesMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ScopePropertiesController : ControllerBase {

        private readonly ILogger<ScopePropertiesController> _logger;
        IConfiguration _config;

        public ScopePropertiesController(IScopeProperties scopeProperties, ILogger<ScopePropertiesController> logger,
            IConfiguration config ) {
            ScopeProperties = scopeProperties;
            _logger = logger;
            _config = config;
        }

        public IScopeProperties ScopeProperties { get; }

        [HttpGet]
        public Dictionary<string,string> Get() {
            var dict = new Dictionary<string, string>();
            dict.Add("User", ScopeProperties.User);
            if(ScopeProperties.Claims != null)
                foreach (var claim in ScopeProperties.Claims)
                    dict.Add(claim.Type, claim.Value);
            if(ScopeProperties.Headers != null)
                foreach (var header in ScopeProperties.Headers)
                    dict.Add(header.Key, header.Value);
            return dict;
        }
    }
}
