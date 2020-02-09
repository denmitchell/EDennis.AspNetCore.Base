using EDennis.AspNetCore.Base.Web;
using Hr.RepoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hr.RepoApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : RepoController<Person, HrContext, PersonRepo> {
        public PersonController(PersonRepo repo) : base(repo) { }
    }
}