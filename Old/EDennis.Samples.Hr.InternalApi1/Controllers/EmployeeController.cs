using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.InternalApi1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.Samples.Hr.InternalApi1.Controllers {
    [Route("iapi/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase {

        EmployeeRepo _repo;

        public EmployeeController(EmployeeRepo repo) {
            _repo = repo;
        }


        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult<Employee> CreateEmployee(
            [FromBody] Employee employee){
            var newEmployee = _repo.Create(employee);
            return Ok(newEmployee);
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(
            [FromRoute] int id) {
            var employee = await _repo.GetByIdAsync(id);
            if (employee == null)
                return NotFound();
            else
                return Ok(employee);
        }


        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000) {
            var employee = await _repo.Query.ToPagedListAsync(pageNumber,pageSize);
            return Ok(employee);
        }

    }
}
