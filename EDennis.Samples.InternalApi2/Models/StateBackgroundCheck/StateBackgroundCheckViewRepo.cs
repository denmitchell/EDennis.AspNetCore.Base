using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.InternalApi2.Models {

    public class StateBackgroundCheckViewRepo 
        : QueryableRepo<StateBackgroundCheckView,
            StateBackgroundCheckContext>{

        public StateBackgroundCheckViewRepo(
            StateBackgroundCheckContext context) 
            : base(context) { }

        public StateBackgroundCheckView GetByEmployeeId(int employeeId) {
            return Context.StateBackgroundCheckViewRecords
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
