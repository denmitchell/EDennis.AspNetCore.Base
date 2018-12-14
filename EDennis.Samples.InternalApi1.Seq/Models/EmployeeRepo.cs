using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.InternalApi1.Models {

    public class EmployeeRepo 
        : ResettableRepo<Employee,HrContext>{

        public EmployeeRepo(HrContext context) 
            : base(context) { }

        public Employee GetByEmployeeId(int employeeId) {
            return Context.Employees
                .Where(e => e.Id == employeeId)
                .FirstOrDefault();
        }
    }
}
