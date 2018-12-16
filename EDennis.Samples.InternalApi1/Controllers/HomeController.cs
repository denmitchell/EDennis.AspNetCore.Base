using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.InternalApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get() {
            return "Home";
        }
    }
}