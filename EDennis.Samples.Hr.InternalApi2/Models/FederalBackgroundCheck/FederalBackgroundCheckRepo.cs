using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

//ReadonlyTemporalRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class FederalBackgroundCheckRepo
        : ReadonlyTemporalRepo<FederalBackgroundCheckView,
            FederalBackgroundCheckContext,
            FederalBackgroundCheckHistoryContext> {

        public FederalBackgroundCheckRepo(
            FederalBackgroundCheckContext context,
            FederalBackgroundCheckHistoryContext historyContext)
            : base(context, historyContext) { }

        public FederalBackgroundCheckView GetLastCheck(int employeeId) {
            return Context.FederalBackgroundChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
