using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class FederalBackgroundCheckRepo 
        : SqlRepo<FederalBackgroundCheckView,
            FederalBackgroundCheckContext>{

        public FederalBackgroundCheckRepo(
            FederalBackgroundCheckContext context) 
            : base(context) { }

        public FederalBackgroundCheckView GetLastCheck(int employeeId) {
            return Context.FederalBackgroundCheckViewRecords
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
