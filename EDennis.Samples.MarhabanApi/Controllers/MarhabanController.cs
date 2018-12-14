using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.MarhabanApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MarhabanController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetMarhaban() {
            return Ok("مرحباً");
        }
    }
}