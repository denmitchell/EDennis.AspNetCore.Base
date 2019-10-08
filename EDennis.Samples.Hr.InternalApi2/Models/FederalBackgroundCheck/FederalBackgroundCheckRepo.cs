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
        public FederalBackgroundCheckRepo(FederalBackgroundCheckContext context, FederalBackgroundCheckHistoryContext historyContext, IScopeProperties scopeProperties, IEnumerable<ILogger<ReadonlyRepo<FederalBackgroundCheckView, FederalBackgroundCheckContext>>> loggers) : base(context, historyContext, scopeProperties, loggers) {
        }

        public FederalBackgroundCheckView GetLastCheck(int employeeId) {
            return Context.FederalBackgroundChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
