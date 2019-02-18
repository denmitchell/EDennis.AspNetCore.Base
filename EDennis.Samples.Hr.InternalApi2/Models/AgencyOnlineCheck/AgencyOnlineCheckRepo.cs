using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyOnlineCheckRepo 
        : SqlRepo<AgencyOnlineCheck,
            AgencyOnlineCheckContext>{

        public AgencyOnlineCheckRepo(
            AgencyOnlineCheckContext context) 
            : base(context) { }

        public AgencyOnlineCheck GetLastCheck(int employeeId) {
            return Context.AgencyOnlineChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
