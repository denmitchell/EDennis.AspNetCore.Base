using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.InternalApi1.Models {

    public class ManagerPositionRepo 
        : QueryableRepo<ManagerPosition,HrContext>{

        public ManagerPositionRepo(HrContext context) 
            : base(context) { }

        public ManagerPosition GetByEmployeeId(int employeeId) {
            return Context.ManagerPositions
                .Where(e => e.EmployeeId == employeeId)
                .FirstOrDefault();
        }
    }
}
