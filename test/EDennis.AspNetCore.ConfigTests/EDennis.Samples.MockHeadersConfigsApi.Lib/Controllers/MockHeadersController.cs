using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.MockHeadersConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class MockHeadersController : ControllerBase {

        private readonly ILogger<MockHeadersController> _logger;
        private readonly IOptionsMonitor<MockHeaderSettingsCollection> _mhsc;
        public MockHeadersController(IOptionsMonitor<MockHeaderSettingsCollection> mhsc, ILogger<MockHeadersController> logger) {
            _logger = logger;
            _mhsc = mhsc;
        }

        [HttpGet]
        public MockHeaderSettingsCollection Get() {
            return _mhsc.CurrentValue;
        }
    }
}
