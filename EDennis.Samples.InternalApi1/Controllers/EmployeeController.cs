using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.InternalApi1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.Samples.InternalApi1.Controllers {
    [Route("iapi/[controller]")]
    [ApiController]
    public class EmployeeController : RepoController {

        EmployeeRepo _repo;

        public EmployeeController(EmployeeRepo repo) {
            _repo = repo;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult<Employee> CreateEmployee(
            [FromBody] Employee employee){
            var newEmployee = _repo.Create(employee);
            return Ok(newEmployee);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(
            [FromRoute] int id) {
            var employee = await _repo.GetByIdAsync(id);
            if (employee == null)
                return NotFound();
            else
                return Ok(employee);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public override void ReplaceRepos(Dictionary<string, DbContextBase> dict) {
            var context = dict["HrContext"] as HrContext;
            _repo = new EmployeeRepo(context);
        }
    }
}
