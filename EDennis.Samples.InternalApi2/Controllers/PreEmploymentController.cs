using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.InternalApi2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.InternalApi2.Controllers {
    [Route("iapi/[controller]")]
    [ApiController]
    public class PreEmploymentController : RepoController {

        public AgencyInvestigatorCheckRepo _agencyInvestigatorCheckRepo { get; set; }
        public AgencyOnlineCheckRepo _agencyOnlineCheckRepo { get; set; }
        public FederalBackgroundCheckViewRepo _federalBackgroundCheckRepo { get; set; }
        public StateBackgroundCheckViewRepo _stateBackgroundCheckRepo { get; set; }

        public PreEmploymentController(AgencyInvestigatorCheckRepo agencyInvestigatorCheckRepo, AgencyOnlineCheckRepo agencyOnlineCheckRepo, FederalBackgroundCheckViewRepo federalBackgroundCheckRepo, StateBackgroundCheckViewRepo stateBackgroundCheckRepo) {
            _agencyInvestigatorCheckRepo = agencyInvestigatorCheckRepo;
            _agencyOnlineCheckRepo = agencyOnlineCheckRepo;
            _federalBackgroundCheckRepo = federalBackgroundCheckRepo;
            _stateBackgroundCheckRepo = stateBackgroundCheckRepo;
        }

        // GET api/preemployment/5
        [HttpGet("{employeeId}")]
        public ActionResult<dynamic> Get(int employeeId) {
            var aic = _agencyInvestigatorCheckRepo.GetByEmployeeId(employeeId);
            var aoc = _agencyOnlineCheckRepo.GetByEmployeeId(employeeId);
            var fbc = _federalBackgroundCheckRepo.GetByEmployeeId(employeeId);
            var sbc = _stateBackgroundCheckRepo.GetByEmployeeId(employeeId);

            var data = DataShaping.CombineCheckData(aic,aoc,fbc,sbc);
            return data;
        }



        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public override void ReplaceRepos(Dictionary<string, DbContextBase> dict) {

            var agencyInvestigatorCheckContext = dict["AgencyInvestigatorCheckContext"] as AgencyInvestigatorCheckContext;
            _agencyInvestigatorCheckRepo = new AgencyInvestigatorCheckRepo(agencyInvestigatorCheckContext);

            var agencyOnlineCheckContext = dict["AgencyOnlineCheckContext"] as AgencyOnlineCheckContext;
            _agencyOnlineCheckRepo = new AgencyOnlineCheckRepo(agencyOnlineCheckContext);

            var stateBackgroundCheckContext = dict["StateBackgroundCheckContext"] as StateBackgroundCheckContext;
            _stateBackgroundCheckRepo = new StateBackgroundCheckViewRepo(stateBackgroundCheckContext);

            var federalBackgroundCheckContext = dict["FederalBackgroundCheckContext"] as FederalBackgroundCheckContext;
            _federalBackgroundCheckRepo = new FederalBackgroundCheckViewRepo(federalBackgroundCheckContext);
        }

    }
}
