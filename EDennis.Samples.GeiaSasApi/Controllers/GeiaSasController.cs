using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.GeiSasApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GeiaSasController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetGeiSas() {
            return Ok("γεια σας");
        }
    }
}