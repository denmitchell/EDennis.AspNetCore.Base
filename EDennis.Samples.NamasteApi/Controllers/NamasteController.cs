using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.NamasteApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class NamasteController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetNamaste() {
            return Ok("నమస్తే");
        }
    }
}