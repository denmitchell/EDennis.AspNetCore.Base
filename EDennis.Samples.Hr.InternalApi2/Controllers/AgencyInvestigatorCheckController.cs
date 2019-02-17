using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.InternalApi2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace EDennis.Samples.Hr.InternalApi2.Controllers {

    [Route("iapi/[controller]")]
    [ApiController]
    public class AgencyInvestigatorCheckController : ControllerBase {

        public AgencyInvestigatorCheckRepo _repo;

        public AgencyInvestigatorCheckController(
            AgencyInvestigatorCheckRepo repo) {
            _repo = repo;
        }

        // GET: api/AgencyInvestigatorCheck/5
        [HttpGet("{employeeId}")]
        public AgencyInvestigatorCheck GetByEmployeeId(int employeeId) {
            return _repo.GetByEmployeeId(employeeId);
        }

        // POST: api/AgencyInvestigatorCheck
        [HttpPost]
        public AgencyInvestigatorCheck Post([FromBody] AgencyInvestigatorCheck check) {
            return _repo.Create(check);
        }
    }
}
