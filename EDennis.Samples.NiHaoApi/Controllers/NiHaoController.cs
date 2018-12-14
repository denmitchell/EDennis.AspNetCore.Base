using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.NiHaoApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class NiHaoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetNiHao() {
            return Ok("你好");
        }
    }
}