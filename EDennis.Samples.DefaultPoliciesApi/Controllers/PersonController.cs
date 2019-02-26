using EDennis.AspNetCore.Base;
using EDennis.Samples.DefaultPoliciesApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DefaultPoliciesApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonRepo _repo;
        private readonly ScopeProperties _scopeProperties;

        public PersonController(PersonRepo repo, ScopeProperties scopeProperties) {
            _repo = repo;
            _scopeProperties = scopeProperties;
        }

        [HttpGet]
        public IEnumerable<Person> GetAll()
        {
            return _repo;
        }

        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return _repo.Where(p=>p.Id==id).FirstOrDefault();
        }

        [HttpPost]
        public void Post([FromBody] Person person)
        {
            _repo.Add(person);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Person person)
        {
            for(int i = 0; i < _repo.Count; i++) {
                if (_repo[i].Id == id) {
                    _repo[i] = person;
                    return;
                }
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var index = -1;
            for (int i = 0; i < _repo.Count; i++) {
                if (_repo[i].Id == id) {
                    index = i;
                    break;
                }
            }
            _repo.RemoveAt(index);
        }
    }
}
