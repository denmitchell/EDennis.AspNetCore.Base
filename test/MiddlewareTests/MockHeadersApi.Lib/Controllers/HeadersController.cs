using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MockHeadersApi.Lib;
using System.Collections.Generic;

namespace EDennis.Samples.MockHeadersMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class HeadersController : ControllerBase {

        private readonly ILogger<HeadersController> _logger;

        public HeadersController(ILogger<HeadersController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string,string>> Get() {
            var headers = HttpContext.Request.Headers;
            var hdrs = new List<KeyValuePair<string,string>>();
            foreach (var header in headers)
                foreach (var value in header.Value.ToArray())
                    hdrs.Add(KeyValuePair.Create(header.Key, value));
            return hdrs;
        }
    }
}
