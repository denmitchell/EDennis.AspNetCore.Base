using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.InternalApi2.Models {

    public class AgencyOnlineCheckRepo 
        : ResettableRepo<AgencyOnlineCheck,
            AgencyOnlineCheckContext>{

        public AgencyOnlineCheckRepo(
            AgencyOnlineCheckContext context) 
            : base(context) { }

        public AgencyOnlineCheck GetByEmployeeId(int employeeId) {
            return Context.AgencyOnlineChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
