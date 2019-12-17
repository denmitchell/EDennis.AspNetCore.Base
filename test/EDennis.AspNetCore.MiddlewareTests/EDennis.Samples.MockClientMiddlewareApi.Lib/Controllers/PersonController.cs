using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.MockClientMiddlewareApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase {
        private readonly ILogger<PersonController> _logger;

        private static List<Person> _persons = new List<Person> {
            new Person { FirstName="Moe", LastName="Stooge"},
            new Person { FirstName="Larry", LastName="Stooge"},
            new Person { FirstName="Curly", LastName="Stooge"}
        };


        public PersonController(ILogger<PersonController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Person> Get() {
            return _persons;
        }


        [HttpGet("{firstName}")]
        public Person GetOne(string firstName) {
            return _persons.FirstOrDefault(x=>x.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase));
        }


        [HttpDelete("{firstName}")]
        public void Delete(string firstName) {
            for (int i = 0; i < _persons.Count; i++) {
                if (_persons[i].FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase))
                    _persons.RemoveAt(i);
                return;
            }
        }
    }
}
