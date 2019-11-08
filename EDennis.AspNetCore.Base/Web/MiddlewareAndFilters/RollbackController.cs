using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EDennis.AspNetCore.Base.Web {

    [Route("api/[controller]")]//controller route
    [ApiController]
    public class RollbackController : ControllerBase {

        [HttpPost("{user}")]
        public async void Index([FromRoute] string user) {
        }

    }
}
