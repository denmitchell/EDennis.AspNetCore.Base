using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class EmployeePositionRepo 
        : SqlRepo<EmployeePosition,HrContext>{

        public EmployeePositionRepo(HrContext context) 
            : base(context) { }
    }
}
