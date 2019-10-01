using EDennis.AspNetCore.Base;
using EDennis.Samples.DefaultPoliciesApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DefaultPoliciesApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase {
        public PersonRepo Repo { get; }
        public ScopeProperties ScopeProperties {get;}

        public PersonController(PersonRepo repo, ScopeProperties scopeProperties) {
            Repo = repo;
            ScopeProperties = scopeProperties;
        }

        [HttpGet]
        public IEnumerable<Person> GetAll()
        {
            return Repo;
        }

        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return Repo.Where(p=>p.Id==id).FirstOrDefault();
        }

        [HttpPost]
        public void Post([FromBody] Person person)
        {
            Repo.Add(person);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Person person)
        {
            for(int i = 0; i < Repo.Count; i++) {
                if (Repo[i].Id == id) {
                    Repo[i] = person;
                    return;
                }
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var index = -1;
            for (int i = 0; i < Repo.Count; i++) {
                if (Repo[i].Id == id) {
                    index = i;
                    break;
                }
            }
            Repo.RemoveAt(index);
        }
    }
}
