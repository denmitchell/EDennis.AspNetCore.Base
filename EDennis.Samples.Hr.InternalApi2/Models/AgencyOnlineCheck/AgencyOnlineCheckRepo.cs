using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

//WriteableRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyOnlineCheckRepo
        : WriteableRepo<AgencyOnlineCheck,
            AgencyOnlineCheckContext> {
        public AgencyOnlineCheckRepo(AgencyOnlineCheckContext context, IScopeProperties scopeProperties, IEnumerable<ILogger<WriteableRepo<AgencyOnlineCheck, AgencyOnlineCheckContext>>> loggers) : base(context, scopeProperties, loggers) {
        }

        public AgencyOnlineCheck GetLastCheck(int employeeId) {
            return Context.AgencyOnlineChecks
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.DateCompleted)
                .FirstOrDefault();
        }
    }
}
