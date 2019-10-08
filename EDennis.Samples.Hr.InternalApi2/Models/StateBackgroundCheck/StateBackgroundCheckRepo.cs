using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

//ReadonlyRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class StateBackgroundCheckRepo
        : ReadonlyRepo<StateBackgroundCheckView,
            StateBackgroundCheckContext> {
        public StateBackgroundCheckRepo(StateBackgroundCheckContext context, IScopeProperties scopeProperties, IEnumerable<ILogger<ReadonlyRepo<StateBackgroundCheckView, StateBackgroundCheckContext>>> loggers) : base(context, scopeProperties, loggers) {
        }

        public StateBackgroundCheckView GetLastCheck(int employeeId) {
            return Context.StateBackgroundChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
