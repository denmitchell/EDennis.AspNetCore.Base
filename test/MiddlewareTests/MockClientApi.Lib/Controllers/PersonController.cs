using Microsoft.AspNetCore.Mvc;

namespace MockClientApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class PersonController : ControllerBase {

        [HttpGet("GetA")]
        public StatusCodeResult GetA()
            => new StatusCodeResult(200);

        [HttpGet("GetB")]
        public StatusCodeResult GetB()
            => new StatusCodeResult(200);

    }
}
