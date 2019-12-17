using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.MockHeadersMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class HeadersController : ControllerBase {

        private readonly ILogger<HeadersController> _logger;

        public HeadersController(ILogger<HeadersController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IHeaderDictionary Get() {
            var headers = HttpContext.Request.Headers;
            return headers;
        }
    }
}
