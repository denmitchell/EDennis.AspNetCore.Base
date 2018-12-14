using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.InternalApi2.Models {

    public class FederalBackgroundCheckViewRepo 
        : QueryableRepo<FederalBackgroundCheckView,
            FederalBackgroundCheckContext>{

        public FederalBackgroundCheckViewRepo(
            FederalBackgroundCheckContext context) 
            : base(context) { }

        public FederalBackgroundCheckView GetByEmployeeId(int employeeId) {
            return Context.FederalBackgroundCheckViewRecords
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
