using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.DefaultPoliciesApi.Models {
    public class PositionRepo : List<Position> {

        public PositionRepo() {
            Add(new Position {
                Id = 1,
                Title = "Manager"
            });
            Add(new Position {
                Id = 2,
                Title = "Supervisor"
            });
            Add(new Position {
                Id = 3,
                Title = "Employee"
            });
        }

    }
}
