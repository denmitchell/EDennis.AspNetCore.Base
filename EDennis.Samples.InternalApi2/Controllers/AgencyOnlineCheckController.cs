using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.InternalApi2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace EDennis.Samples.InternalApi2.Controllers {

    [Route("iapi/[controller]")]
    [ApiController]
    public class AgencyOnlineCheckController : RepoController {

        public AgencyOnlineCheckRepo _repo;

        public AgencyOnlineCheckController(
            AgencyOnlineCheckRepo repo) {
            _repo = repo;
        }

        // GET: api/AgencyOnlineCheck/5
        [HttpGet("{employeeId}")]
        public AgencyOnlineCheck GetByEmployeeId(int employeeId) {
            var result = _repo.GetByEmployeeId(employeeId);
            return result;
        }

        // POST: api/AgencyOnlineCheck
        [HttpPost]
        public AgencyOnlineCheck Post([FromBody] AgencyOnlineCheck check) {
            return _repo.Create(check);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public override void ReplaceRepos(Dictionary<string, DbContextBase> dict) {
            var agencyOnlineCheckContext = dict["AgencyOnlineCheckContext"] as AgencyOnlineCheckContext;
            _repo = new AgencyOnlineCheckRepo(agencyOnlineCheckContext);
        }
    }
}
