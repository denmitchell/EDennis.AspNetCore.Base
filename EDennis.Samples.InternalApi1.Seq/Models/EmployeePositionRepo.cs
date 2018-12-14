using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.InternalApi1.Models {

    public class EmployeePositionRepo 
        : ResettableRepo<EmployeePosition,HrContext>{

        public EmployeePositionRepo(HrContext context) 
            : base(context) { }
    }
}
