using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class PositionRepo 
        : WriteableTemporalRepo<Position,HrContext,HrHistoryContext>{

        public PositionRepo(HrContext context, HrHistoryContext historyContext,
            ScopeProperties scopeProperties) 
            : base(context,historyContext,scopeProperties) { }

        public List<Position> GetByEmployeeId(int employeeId) {
            return Context.Positions
                .Where(e => e.EmployeePositions
                    .Any(ep => ep.EmployeeId == employeeId))
                .ToList();
        }
    }
}
