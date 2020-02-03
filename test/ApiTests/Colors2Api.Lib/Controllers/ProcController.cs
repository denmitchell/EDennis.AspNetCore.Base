using Colors2.Models;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;

namespace Colors2Api.Lib.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class ProcController : SqlServerStoredProcedureController<Color2DbContext> {
        public ProcController(Color2DbContext context) : base(context) {
        }
    }
}
