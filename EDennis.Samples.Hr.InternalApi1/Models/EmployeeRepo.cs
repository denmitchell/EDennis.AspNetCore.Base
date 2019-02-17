using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class EmployeeRepo 
        : SqlRepo<Employee,HrContext>{

        public EmployeeRepo(HrContext context) 
            : base(context) { }

        public Employee GetByEmployeeId(int employeeId) {
            return Context.Employees
                .Where(e => e.Id == employeeId)
                .FirstOrDefault();
        }
    }
}
