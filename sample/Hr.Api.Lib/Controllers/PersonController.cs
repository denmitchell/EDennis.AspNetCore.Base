using EDennis.AspNetCore.Base.Web;
using Hr.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PersonController : RepoController<Person, HrContext, PersonRepo> {
        public PersonController(PersonRepo repo) : base(repo) { }
    }
}