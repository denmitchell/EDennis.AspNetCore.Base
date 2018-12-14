using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.HelloApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetHello() {
            return Ok("Hello");
        }
    }
}