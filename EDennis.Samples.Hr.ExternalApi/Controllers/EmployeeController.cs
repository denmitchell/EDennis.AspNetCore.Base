using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.Hr.ExternalApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ProxyController {

        InternalApi1 _internalApi1;
        InternalApi2 _internalApi2;

        public EmployeeController(
            InternalApi1 internalApi1,
            InternalApi2 internalApi2) {
            _internalApi1 = internalApi1;
            _internalApi2 = internalApi2;
        }



        [HttpPost]
        public ActionResult<Employee> CreateEmployee(
            [FromBody] Employee employee){
            var newEmployee = _internalApi1.CreateEmployee(employee);
            return Ok(newEmployee);
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(
            [FromRoute] int id) {
            var employee = _internalApi1.GetEmployee(id);
            if (employee == null)
                return NotFound();
            else
                return Ok(employee);
        }


        [HttpPost("agencyonlinecheck")]
        public ActionResult<AgencyOnlineCheck> CreateAgencyOnlineCheck(
            [FromBody] AgencyOnlineCheck check) {
            var newCheck = _internalApi2.CreateAgencyOnlineCheck(check);
            return Ok(newCheck);
        }


        [HttpPost("agencyinvestigatorcheck")]
        public ActionResult<AgencyInvestigatorCheck> CreateAgencyInvestigatorCheck(
            [FromBody] AgencyInvestigatorCheck check) {
            var newCheck = _internalApi2.CreateAgencyInvestigatorCheck(check);
            return Ok(newCheck);
        }

        [HttpGet("preemployment/{id}")]
        public ActionResult<dynamic> GetPreEmploymentChecks(
            [FromRoute] int id) {
            var checks = _internalApi2.GetPreEmploymentChecks(id);
            return Ok(checks);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public override IApiProxy[] GetApis() {
            return new IApiProxy[] { _internalApi1, _internalApi2 };
        }
    }
}
