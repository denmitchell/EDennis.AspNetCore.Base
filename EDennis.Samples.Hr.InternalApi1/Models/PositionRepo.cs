using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class PositionRepo
        : WriteableTemporalRepo<Position,HrContext,HrHistoryContext>{
        public PositionRepo(HrContext context, HrHistoryContext historyContext, IScopeProperties scopeProperties, IEnumerable<ILogger<WriteableRepo<Position, HrContext>>> loggers) : base(context, historyContext, scopeProperties, loggers) {
        }

        public List<Position> GetByEmployeeId(int employeeId) {
            return Context.Positions
                .Where(e => e.EmployeePositions
                    .Any(ep => ep.EmployeeId == employeeId))
                .ToList();
        }
    }
}
