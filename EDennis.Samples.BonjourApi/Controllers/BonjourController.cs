using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.BonjourApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BonjourController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetBonjour() {
            return Ok("Bonjour");
        }
    }
}