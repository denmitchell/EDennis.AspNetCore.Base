using EDennis.AspNetCore.Base.Web;
using Hr.RepoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hr.RepoApi.Lib.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ProcController : SqlServerStoredProcedureController<HrContext> {
        public ProcController(HrContext context) : base(context) {        }
    }
}
