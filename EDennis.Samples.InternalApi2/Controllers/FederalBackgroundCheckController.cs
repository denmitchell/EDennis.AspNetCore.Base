using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.InternalApi2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace EDennis.Samples.InternalApi2.Controllers {

    [Route("iapi/[controller]")]
    [ApiController]
    public class FederalBackgroundCheckController {

        public FederalBackgroundCheckViewRepo _repo;

        public FederalBackgroundCheckController(
            FederalBackgroundCheckViewRepo repo) {
            _repo = repo;
        }

        // GET: api/FederalBackgroundCheck/5
        [HttpGet("{employeeId}")]
        public FederalBackgroundCheckView GetByEmployeeId(int employeeId) {
            return _repo.GetByEmployeeId(employeeId);
        }

    }
}
