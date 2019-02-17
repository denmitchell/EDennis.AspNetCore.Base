using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class StateBackgroundCheckRepo 
        : SqlRepo<StateBackgroundCheckView,
            StateBackgroundCheckContext>{

        public StateBackgroundCheckRepo(
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
