using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.WitajApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class WitajController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetWitaj() {
            return Ok("Witaj");
        }
    }
}