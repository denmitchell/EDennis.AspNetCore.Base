using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyInvestigatorCheckRepo 
        : SqlRepo<AgencyInvestigatorCheck,
            AgencyInvestigatorCheckContext>{

        public AgencyInvestigatorCheckRepo(
            AgencyInvestigatorCheckContext context) 
            : base(context) { }

        public AgencyInvestigatorCheck GetLastCheck(int employeeId) {
            return Context.AgencyInvestigatorChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e=>e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
