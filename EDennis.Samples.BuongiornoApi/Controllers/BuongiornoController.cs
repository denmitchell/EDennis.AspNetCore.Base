using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.BuongiornoApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BuongiornoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetBuongiorno() {
            return Ok("Buongiorno");
        }
    }
}