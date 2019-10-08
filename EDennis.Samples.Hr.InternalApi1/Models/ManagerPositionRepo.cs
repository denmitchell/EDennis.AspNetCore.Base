using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class ManagerPositionRepo
        : ReadonlyRepo<ManagerPosition,HrContext>{
        public ManagerPositionRepo(HrContext context, IScopeProperties scopeProperties, IEnumerable<ILogger<ReadonlyRepo<ManagerPosition, HrContext>>> loggers) : base(context, scopeProperties, loggers) {
        }

        public ManagerPosition GetByEmployeeId(int employeeId) {
            return Context.ManagerPositions
                .Where(e => e.EmployeeId == employeeId)
                .FirstOrDefault();
        }
    }
}
