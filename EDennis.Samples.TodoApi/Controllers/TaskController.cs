using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.TodoApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.Samples.TodoApi.Controllers {

    [Route("iapi/[controller]")]
    [ApiController]
    public class TaskController : RepoController {

        private TaskRepo _repo;

        public TaskController(TaskRepo repo) {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Models.Task>> GetAll() {
            return _repo.GetByLinq(a=>1==1,1,1000);
        }


        [HttpGet("{id}")]
        public ActionResult<Models.Task> Get(int id) {
            return _repo.GetById(id);
        }

        [HttpGet("async/{id}")]
        public async Task<ActionResult<Models.Task>> GetAsync(int id) {
            return await _repo.GetByIdAsync(id);
        }

        [HttpPost]
        public Models.Task Post([FromBody] Models.Task task) {
            return _repo.Create(task);
        }

        [HttpPost("async")]
        public async System.Threading.Tasks.Task<Models.Task> PostAsync([FromBody] Models.Task task) {
            return await _repo.CreateAsync(task);
        }

        [HttpPut("{id}")]
        public Models.Task Put(int id, [FromBody] Models.Task task) {
            return _repo.Update(task);
        }

        [HttpPut("async/{id}")]
        public async System.Threading.Tasks.Task<Models.Task> PutAsync(
            int id, [FromBody] Models.Task task) {
            return await _repo.UpdateAsync(task);
        }

        [HttpDelete("{id}")]
        public void Delete(int id) {
            _repo.Delete(id);
        }

        [HttpDelete("async/{id}")]
        public async System.Threading.Tasks.Task DeleteAsync(int id) {
            await _repo.DeleteAsync(id);
        }


        public override void ReplaceRepos(Dictionary<string, DbContextBase> dict) {
            var context = dict[typeof(TodoContext).Name] as TodoContext;
            _repo = new TaskRepo(context);
        }
    }
}
