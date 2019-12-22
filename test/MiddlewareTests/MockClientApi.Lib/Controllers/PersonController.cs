using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.MockClientMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase {

        [HttpGet("GetA")]
        public StatusCodeResult GetA()
            => new StatusCodeResult(200);

        [HttpGet("GetB")]
        public StatusCodeResult GetB()
            => new StatusCodeResult(200);

    }
}
