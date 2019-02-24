using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class FederalBackgroundCheckRepo
        : ReadonlyRepo<FederalBackgroundCheck,
            FederalBackgroundCheckContext> {

        public FederalBackgroundCheckRepo(
            FederalBackgroundCheckContext context)
            : base(context) { }

        public FederalBackgroundCheck GetLastCheck(int employeeId) {
            return Context.FederalBackgroundChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
