using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.HolaApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HolaController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetHola() {
            return Ok("Hola");
        }
    }
}