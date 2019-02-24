using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class StateBackgroundCheckRepo
        : ReadonlyRepo<StateBackgroundCheck,
            StateBackgroundCheckContext> {

        public StateBackgroundCheckRepo(
            StateBackgroundCheckContext context)
            : base(context) { }

        public StateBackgroundCheck GetLastCheck(int employeeId) {
            return Context.StateBackgroundChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
