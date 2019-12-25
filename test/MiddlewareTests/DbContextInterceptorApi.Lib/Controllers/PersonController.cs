using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase {

        private readonly ILogger<PersonController> _logger;
        private readonly PersonRepo _repo;

        public PersonController(PersonRepo repo, ILogger<PersonController> logger) {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public List<Person> Get() => _repo.Context.Person.ToList();

        [HttpGet("{id}")]
        public Person Get(int id) => _repo.GetById(id);

        [HttpDelete("{id}")]
        public void Delete(int id) => _repo.Delete(id);

        [HttpPut("{id}")]
        public void Update(Person person, int id) => _repo.Update(person,id);

        [HttpPost]
        public void Create(Person person) => _repo.Create(person);


    }
}
