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


        [HttpPost("Reset")]
        public void Reset() {
            Persons.Clear();
            Persons.AddRange (new List<Person>
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
            });
        }

        [HttpGet]
        public IEnumerable<Person> Get() {
            return Persons;
        }

        [HttpGet("{id}")]
        public Person Get(int id) {
            return Persons.FirstOrDefault(p => p.Id == id);
        }

        [HttpGet("IdNot")]
        public IEnumerable<Person> IdNot([FromQuery]int idNot) {
            return Persons.Where(p => p.Id != idNot);
        }

        [HttpDelete("{id}")]
        public void Delete(int id) {
            var person = Get(id);
            Persons.Remove(person);
        }

        [HttpPut("{id}")]
        public ActionResult<Person> Update(Person person, [FromRoute]int id) {
            if (Persons.Count(p => p.Id == id) == 0)
                return new StatusCodeResult(400);
            else {
                var rec = Get(id);
                rec.FirstName = person.FirstName;
                rec.LastName = person.LastName;
                return new OkObjectResult(rec);
            }
        }

        [HttpPost]
        public Person Create(Person person) {
            Persons.Add(person);
            return person;
        }
    }
}
