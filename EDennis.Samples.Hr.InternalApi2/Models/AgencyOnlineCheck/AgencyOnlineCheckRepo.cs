using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

//WriteableRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyOnlineCheckRepo
        : WriteableRepo<AgencyOnlineCheck,
            AgencyOnlineCheckContext> {

        public AgencyOnlineCheckRepo(
            AgencyOnlineCheckContext context,
            ScopeProperties scopeProperties)
            : base(context, scopeProperties) { }

        public AgencyOnlineCheck GetLastCheck(int employeeId) {
            return Context.AgencyOnlineChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
