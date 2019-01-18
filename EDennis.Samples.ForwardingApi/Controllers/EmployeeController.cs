using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.ForwardingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace EDennis.Samples.ForwardingApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ProxyController {

        InternalApi1 _internalApi1;

        public EmployeeController(
            InternalApi1 internalApi1) {
            _internalApi1 = internalApi1;
        }



        [HttpPost]
        public HttpResponseMessage CreateEmployee([FromBody] Employee employee){
            var newEmployee = _internalApi1.CreateEmployee(employee);
            var msg = new HttpResponseMessage();
            msg.Content = new BodyContent<Employee>(newEmployee);
            return msg;
        }


        [HttpPost("via-forward")]
        public async Task<ActionResult<Employee>> CreateEmployeeViaForward([FromBody] Employee employee) {
            var emp = await _internalApi1.CreateEmployeeAsync(HttpContext.Request,employee);
            return emp;
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

        [HttpGet("via-forward/{id}")]
        public ActionResult<Employee> GetEmployeeViaForward([FromRoute] int id) {
            var employee = _internalApi1.GetEmployeeAsync(HttpContext.Request,id);
            if (employee == null)
                return NotFound();
            else
                return Ok(employee);
        }

        [HttpPut("{id}")]
        public ActionResult<Employee> PutEmployee(
            [FromBody] Employee employee, [FromRoute] int id) {
            var emp = _internalApi1.UpdateEmployee(employee,id);
            if (emp == null)
                return NotFound();
            else
                return Ok(employee);
        }

        [HttpDelete("{id}")]
        public void DeleteEmployee(
            [FromRoute] int id) {
            _internalApi1.DeleteEmployee(id);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public override IApiProxy[] GetApis() {
            return new IApiProxy[] { _internalApi1 };
        }
    }
}
