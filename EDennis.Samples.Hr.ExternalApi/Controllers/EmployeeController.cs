using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.ExternalApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EDennis.Samples.Hr.ExternalApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase {

        InternalApi1 _api1;
        InternalApi2 _api2;

        public EmployeeController(InternalApi1 api1,InternalApi2 api2) {
            _api1 = api1;
            _api2 = api2;
        }



        [HttpPost]
        public ActionResult CreateEmployee(
            [FromBody] Employee employee){
            var response = _api1.CreateEmployee(employee);
            return new StatusCodeResult(response.StatusCodeValue);
        }


        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(
            [FromRoute] int id) {
            var response = _api1.GetEmployee(id);
            if (response.StatusCodeValue > 299)
                return new StatusCodeResult(response.StatusCodeValue);
            else {
                return response.Value;
            }
        }


        [HttpGet]
        public ActionResult<List<Employee>> GetEmployees(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000) {
            var response = _api1.GetEmployees(pageNumber, pageSize);
            if (response.StatusCodeValue > 299)
                return new StatusCodeResult(response.StatusCodeValue);
            else {
                return response.Value;
            }
        }


        [HttpPost("agencyonlinecheck")]
        public ActionResult CreateAgencyOnlineCheck(
            [FromBody] AgencyOnlineCheck check) {
            var response = _api2.CreateAgencyOnlineCheck(check);
            return new StatusCodeResult(response.StatusCodeValue);
        }


        [HttpPost("agencyinvestigatorcheck")]
        public ActionResult CreateAgencyInvestigatorCheck(
            [FromBody] AgencyInvestigatorCheck check) {
            var response = _api2.CreateAgencyInvestigatorCheck(check);
            return new StatusCodeResult(response.StatusCodeValue);
        }

        [HttpGet("preemployment/{id}")]
        public ActionResult<dynamic> GetPreEmploymentChecks(
            [FromRoute] int id) {
            var response = _api2.GetPreEmploymentChecks(id);
            if (response.StatusCodeValue > 299)
                return new StatusCodeResult(response.StatusCodeValue);
            else {
                return response.Value;
            }
        }

    }
}
