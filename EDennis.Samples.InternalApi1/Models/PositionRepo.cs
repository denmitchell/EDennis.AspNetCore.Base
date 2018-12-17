using EDennis.AspNetCore.Base.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.InternalApi1.Models {

    public class PositionRepo 
        : WriteableRepo<Position,HrContext>{

        public PositionRepo(HrContext context) 
            : base(context) { }

        public List<Position> GetByEmployeeId(int employeeId) {
            return Context.Positions
                .Where(e => e.EmployeePositions
                    .Any(ep => ep.EmployeeId == employeeId))
                .ToList();
        }
    }
}
