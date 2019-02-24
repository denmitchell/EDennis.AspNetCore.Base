using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class EmployeeRepo
        : WriteableTemporalRepo<Position, HrContext, HrHistoryContext> {

        public EmployeeRepo(HrContext context, HrHistoryContext historyContext,
            ScopeProperties scopeProperties)
            : base(context, historyContext, scopeProperties) { }

    }
}
