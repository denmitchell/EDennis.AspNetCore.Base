using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.InternalApi2.Models {

    public class AgencyInvestigatorCheckRepo 
        : WriteableRepo<AgencyInvestigatorCheck,
            AgencyInvestigatorCheckContext>{

        public AgencyInvestigatorCheckRepo(
            AgencyInvestigatorCheckContext context) 
            : base(context) { }

        public AgencyInvestigatorCheck GetByEmployeeId(int employeeId) {
            return Context.AgencyInvestigatorChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e=>e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
