using Colors2.Models;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;

namespace Colors2Api.Lib.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HslController : QueryController<Hsl, Color2DbContext, HslRepo> {
        public HslController(HslRepo repo) : base(repo) { }
    }
}