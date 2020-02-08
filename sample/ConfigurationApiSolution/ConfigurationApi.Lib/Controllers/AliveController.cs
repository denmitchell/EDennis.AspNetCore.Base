using Microsoft.AspNetCore.Mvc;

namespace ConfigurationApi.Lib.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AliveController : ControllerBase {
        
        [HttpGet]
        public IActionResult Alive() => Ok(true);

    }
}
