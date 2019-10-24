using System.Collections.Generic;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class EmployeePositionRepo
        : WriteableTemporalRepo<EmployeePosition, HrContext, HrHistoryContext> {
        public EmployeePositionRepo(HrContext context, HrHistoryContext historyContext, 
            ScopeProperties22 scopeProperties, 
            ILogger<WriteableRepo<EmployeePosition, HrContext>> logger) 
            : base(context, historyContext, scopeProperties, logger) {
        }
    }
}
