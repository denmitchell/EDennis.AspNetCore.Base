using System.Collections.Generic;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class EmployeePositionRepo
        : WriteableTemporalRepo<EmployeePosition, HrContext, HrHistoryContext> {
        public EmployeePositionRepo(HrContext context, HrHistoryContext historyContext, IScopeProperties scopeProperties, IEnumerable<ILogger<WriteableRepo<EmployeePosition, HrContext>>> loggers) : base(context, historyContext, scopeProperties, loggers) {
        }
    }
}
