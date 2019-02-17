using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.InternalApi2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.Hr.InternalApi2.Controllers {
    [Route("iapi/[controller]")]
    [ApiController]
    public class PreEmploymentController : ControllerBase {

        public AgencyInvestigatorCheckRepo _agencyInvestigatorCheckRepo { get; set; }
        public AgencyOnlineCheckRepo _agencyOnlineCheckRepo { get; set; }
        public FederalBackgroundCheckRepo _federalBackgroundCheckRepo { get; set; }
        public StateBackgroundCheckRepo _stateBackgroundCheckRepo { get; set; }

        public PreEmploymentController(AgencyInvestigatorCheckRepo agencyInvestigatorCheckRepo, AgencyOnlineCheckRepo agencyOnlineCheckRepo, FederalBackgroundCheckRepo federalBackgroundCheckRepo, StateBackgroundCheckRepo stateBackgroundCheckRepo) {
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

    }
}
