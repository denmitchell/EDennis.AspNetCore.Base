using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class EmployeePositionRepo 
        : WriteableTemporalRepo<EmployeePosition,HrContext,HrHistoryContext>{

        public EmployeePositionRepo(HrContext context, HrHistoryContext historyContext,
            ScopeProperties scopeProperties) 
            : base(context,historyContext,scopeProperties) { }
    }
}
