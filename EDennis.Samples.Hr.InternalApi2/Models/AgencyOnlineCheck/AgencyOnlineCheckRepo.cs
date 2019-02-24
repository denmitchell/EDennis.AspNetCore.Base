using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyOnlineCheckRepo
        : WriteableTemporalRepo<AgencyOnlineCheck,
            AgencyOnlineCheckContext,
            AgencyOnlineCheckHistoryContext> {

        public AgencyOnlineCheckRepo(
            AgencyOnlineCheckContext context,
            AgencyOnlineCheckHistoryContext historyContext,
            ScopeProperties scopeProperties)
            : base(context, historyContext, scopeProperties) { }

        public AgencyOnlineCheck GetLastCheck(int employeeId) {
            return Context.AgencyOnlineChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
