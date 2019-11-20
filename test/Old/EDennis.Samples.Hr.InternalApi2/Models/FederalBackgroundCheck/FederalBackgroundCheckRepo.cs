using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

//ReadonlyTemporalRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class FederalBackgroundCheckRepo
        : ReadonlyTemporalRepo<FederalBackgroundCheckView,
            FederalBackgroundCheckContext,
            FederalBackgroundCheckHistoryContext> {
        public FederalBackgroundCheckRepo(FederalBackgroundCheckContext context, 
            FederalBackgroundCheckHistoryContext historyContext, 
            ScopeProperties22 scopeProperties, 
            ILogger<ReadonlyRepo<FederalBackgroundCheckView, FederalBackgroundCheckContext>> logger) 
            : base(context, historyContext, scopeProperties, logger) {
        }

        public FederalBackgroundCheckView GetLastCheck(int employeeId) {
            return Context.FederalBackgroundChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
