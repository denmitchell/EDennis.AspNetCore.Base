using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.InternalApi2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace EDennis.Samples.InternalApi2.Controllers {

    [Route("iapi/[controller]")]
    [ApiController]
    public class AgencyInvestigatorCheckController : RepoController {

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

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public override void ReplaceRepos(Dictionary<string, DbContextBase> dict) {
            var agencyInvestigatorCheckContext = dict["AgencyInvestigatorCheckContext"] as AgencyInvestigatorCheckContext;
            _repo = new AgencyInvestigatorCheckRepo(agencyInvestigatorCheckContext);
        }
    }
}
