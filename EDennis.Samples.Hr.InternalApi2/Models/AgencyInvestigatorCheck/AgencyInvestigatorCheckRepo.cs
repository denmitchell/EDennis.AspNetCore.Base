using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

//WriteableTemporalRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyInvestigatorCheckRepo
        : WriteableTemporalRepo<AgencyInvestigatorCheck,
            AgencyInvestigatorCheckContext,
            AgencyInvestigatorCheckHistoryContext>{
        public AgencyInvestigatorCheckRepo(AgencyInvestigatorCheckContext context, AgencyInvestigatorCheckHistoryContext historyContext, IScopeProperties scopeProperties, IEnumerable<ILogger<WriteableRepo<AgencyInvestigatorCheck, AgencyInvestigatorCheckContext>>> loggers) : base(context, historyContext, scopeProperties, loggers) {
        }

        public AgencyInvestigatorCheck GetLastCheck(int employeeId) {
            return Context.AgencyInvestigatorChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e=>e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
