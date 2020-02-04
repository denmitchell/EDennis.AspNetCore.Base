using Colors2.Models;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;

namespace Colors2Api.Lib.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RgbController : RepoController<Rgb, Color2DbContext, RgbRepo>{
        public RgbController(RgbRepo repo) : base(repo) { }
    }
}
