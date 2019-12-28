using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace PkRewriterApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase {

        private static readonly List<Person> Persons = new List<Person>
        {
            new Person {
                Id = -999001,
                FirstName = "Bob",
                LastName = "Barker"
            },
            new Person {
                Id = -999002,
                FirstName = "Monty",
                LastName = "Hall"
            }
        };

        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Person> Get() {
            return Persons;
        }

        [HttpGet("{id}")]
        public Person Get(int id) {
            return Persons.FirstOrDefault(p => p.Id == id);
        }

        [HttpGet("{idNot}")]
        public IEnumerable<Person> IdNot([FromQuery]int idNot) {
            return Persons.Where(p => p.Id != idNot);
        }

        [HttpDelete("{id}")]
        public void Delete(int id) {
            var person = Get(id);
            Persons.Remove(person);
        }

        [HttpPut("{id}")]
        public Person Update(Person person, int id) {
            var rec = Get(id);
            rec.FirstName = person.FirstName;
            rec.LastName = person.LastName;
            return rec;
        }

        [HttpPost]
        public Person Create(Person person) {
            Persons.Add(person);
            return person;
        }
    }
}
