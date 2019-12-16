using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.MockClientConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class MockClientController : ControllerBase {

        private readonly ILogger<MockClientController> _logger;
        private readonly ISecureTokenService _secureTokenService;
        private readonly IOptionsMonitor<ActiveMockClientSettings> _activeMockClientSettings;

        public MockClientController(IOptionsMonitor<ActiveMockClientSettings> activeMockClientSettings, ISecureTokenService secureTokenService, ILogger<MockClientController> logger) {
            _logger = logger;
            _activeMockClientSettings = activeMockClientSettings;
            _secureTokenService = secureTokenService;
        }

        [HttpGet]
        public ActiveMockClientSettings Get() {
            return _activeMockClientSettings.CurrentValue;
        }
    }
}
