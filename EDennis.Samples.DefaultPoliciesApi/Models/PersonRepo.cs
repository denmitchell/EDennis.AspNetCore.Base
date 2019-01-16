using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.DefaultPoliciesApi.Models {
    public class PersonRepo : List<Person> {

        public PersonRepo() {
            Add(new Person {
                Id = 1,
                Name = "Moe"
            });
            Add(new Person 
            {
                Id = 2,
                Name = "Larry"
            });
            Add(new Person {
                Id = 3,
                Name = "Curly"
            });
        }
    }
}
