using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyInvestigatorCheckRepo 
        : WriteableTemporalRepo<AgencyInvestigatorCheck,
            AgencyInvestigatorCheckContext,
            AgencyInvestigatorCheckHistoryContext>{

        public AgencyInvestigatorCheckRepo(
            AgencyInvestigatorCheckContext context,
            AgencyInvestigatorCheckHistoryContext historyContext,
            ScopeProperties scopeProperties) 
            : base(context,historyContext, scopeProperties) { }

        public AgencyInvestigatorCheck GetLastCheck(int employeeId) {
            return Context.AgencyInvestigatorChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e=>e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
