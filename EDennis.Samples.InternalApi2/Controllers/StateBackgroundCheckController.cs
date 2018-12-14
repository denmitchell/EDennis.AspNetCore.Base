using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.InternalApi2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace EDennis.Samples.InternalApi2.Controllers {

    [Route("iapi/[controller]")]
    [ApiController]
    public class StateBackgroundCheckController {

        public StateBackgroundCheckViewRepo _repo;

        public StateBackgroundCheckController(
            StateBackgroundCheckViewRepo repo) {
            _repo = repo;
        }

        // GET: api/StateBackgroundCheck/5
        [HttpGet("{employeeId}")]
        public StateBackgroundCheckView GetByEmployeeId(int employeeId) {
            return _repo.GetByEmployeeId(employeeId);
        }

    }
}
