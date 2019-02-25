using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class EmployeeRepo
        : WriteableTemporalRepo<Employee, HrContext, HrHistoryContext> {

        public EmployeeRepo(HrContext context, HrHistoryContext historyContext,
            ScopeProperties scopeProperties)
            : base(context, historyContext, scopeProperties) { }

    }
}
