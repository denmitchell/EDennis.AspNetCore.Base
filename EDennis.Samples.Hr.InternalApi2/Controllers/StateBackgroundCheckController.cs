using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.InternalApi2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace EDennis.Samples.Hr.InternalApi2.Controllers {

    [Route("iapi/[controller]")]
    [ApiController]
    public class StateBackgroundCheckController {

        public StateBackgroundCheckRepo _repo;

        public StateBackgroundCheckController(
            StateBackgroundCheckRepo repo) {
            _repo = repo;
        }

        // GET: api/StateBackgroundCheck/5
        [HttpGet("{employeeId}")]
        public StateBackgroundCheckView GetLastCheck(int employeeId) {
            return _repo.GetLastCheck(employeeId);
        }

    }
}
