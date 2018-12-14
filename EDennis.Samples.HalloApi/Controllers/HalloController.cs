using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.HalloApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HalloController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetHallo() {
            return Ok("Hallo");
        }
    }
}