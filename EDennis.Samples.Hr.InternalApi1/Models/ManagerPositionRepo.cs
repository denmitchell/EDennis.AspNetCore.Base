using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class ManagerPositionRepo 
        : SqlRepo<ManagerPosition,HrContext>{

        public ManagerPositionRepo(HrContext context) 
            : base(context) { }

        public ManagerPosition GetByEmployeeId(int employeeId) {
            return Context.ManagerPositions
                .Where(e => e.EmployeeId == employeeId)
                .FirstOrDefault();
        }
    }
}
