using System.Collections.Generic;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class EmployeeRepo
        : WriteableTemporalRepo<Employee, HrContext, HrHistoryContext> {
        public EmployeeRepo(HrContext context, HrHistoryContext historyContext, IScopeProperties scopeProperties, IEnumerable<ILogger<WriteableRepo<Employee, HrContext>>> loggers) : base(context, historyContext, scopeProperties, loggers) {
        }
    }
}
