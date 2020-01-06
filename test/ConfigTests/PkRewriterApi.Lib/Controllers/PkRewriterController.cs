using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PkRewriterApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PkRewriterController : ControllerBase {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<PkRewriterController> _logger;
        private readonly PkRewriterSettings _settings;
        public PkRewriterController(IOptionsMonitor<PkRewriterSettings> settings, ILogger<PkRewriterController> logger) {
            _logger = logger;
            _settings = settings.CurrentValue;
        }

        [HttpGet]
        public PkRewriterSettings Get() {
            return _settings;
        }
    }
}
