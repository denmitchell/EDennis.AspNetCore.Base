using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

//ReadonlyRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class StateBackgroundCheckRepo
        : ReadonlyRepo<StateBackgroundCheckView,
            StateBackgroundCheckContext> {

        public StateBackgroundCheckRepo(
            StateBackgroundCheckContext context)
            : base(context) { }

        public StateBackgroundCheckView GetLastCheck(int employeeId) {
            return Context.StateBackgroundChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
