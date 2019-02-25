using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class EmployeePositionRepo 
        : WriteableTemporalRepo<EmployeePosition,HrContext,HrHistoryContext>{

        public EmployeePositionRepo(HrContext context, HrHistoryContext historyContext,
            ScopeProperties scopeProperties) 
            : base(context,historyContext,scopeProperties) { }
    }
}
