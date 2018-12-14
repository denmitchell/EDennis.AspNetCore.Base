using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.InternalApi1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.Samples.InternalApi1.Seq.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : RepoController {

        EmployeeRepo _repo;

        public EmployeeController(EmployeeRepo repo) {
            _repo = repo;
        }

        public async Task<ActionResult<Employee>> CreateEmployee(
            [FromBody] Employee employee){
            var newEmployee = await _repo.CreateAsync(employee);
            return Ok(newEmployee);
        }

        public override void ReplaceRepos(Dictionary<string, DbContextBase> dict) {
            var context = dict["HrContext"] as HrContext;
            _repo = new EmployeeRepo(context);
        }
    }
}
